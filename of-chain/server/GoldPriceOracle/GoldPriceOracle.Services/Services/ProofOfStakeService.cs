using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Infrastructure.Cryptography.RandomGenerator;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Infrastructure.Utils.Helpers;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.ProofOfStake;
using GoldPriceOracle.Services.Models.Voting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class ProofOfStakeService : IProofOfStakeService, IBlockchainMonitorService
    {
        private const string CURRENT_ERA_NOT_FOUND = "Current era not found";
        private const string NODE_DATA_NOT_FOUND = "Node is not set up. You need to set the node";
        private const string NODE_IS_NOT_PREVIOUS_CHAIRMAN = "Node is not previous chairman. Cannot propose new election";
        private const string NO_STAKEHOLDERS_FOUND = "No validator stakeholders found";
        private const string NEW_ERA_COUNCIL_PROPOSED = "New era council had been proposed";
        private const string ALREADY_VOTED = "Already voted as chairman";
        private const string NODE_SUCCESSFULLY_VOTE = "Node successfully vote";
        private const string ERA_NOT_FOUND = "Era not found";

        private readonly IProofOfStakeTokenService _proofOfStakeTokenService;
        private readonly INodeDataDataAccessService _nodeDataDataAccessService;
        private readonly IDeterministicRandomGenerator _deterministicRandomGenerator;

        public ProofOfStakeService(IProofOfStakeTokenService proofOfStakeTokenService,
            INodeDataDataAccessService nodeDataDataAccessService,
            IDeterministicRandomGenerator deterministicRandomGenerator)
        {
            _proofOfStakeTokenService = proofOfStakeTokenService;
            _nodeDataDataAccessService = nodeDataDataAccessService;
            _deterministicRandomGenerator = deterministicRandomGenerator;
        }

        public async Task<TryResult<VotingResult>> TryProposeNewEraElectionAsync(string utcTimeStamp, string eraId)
        {
            var currentEra = await _proofOfStakeTokenService.GetCurrentEraAsync();
            if (currentEra == null ||
                currentEra.CurrentEra == null
                || currentEra.CurrentEra.Chairman == null)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, CURRENT_ERA_NOT_FOUND));
            }
            var nodeData = _nodeDataDataAccessService.GetNodeData();

            if (nodeData == null)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NODE_DATA_NOT_FOUND));
            }
            var isFirstEra = currentEra.CurrentEra.Chairman.IsNullAddress();
            if (!isFirstEra && !nodeData.ActiveAddress.IsAddressEqualTo(currentEra.CurrentEra.Chairman))
            {
                return TryResult<VotingResult>.Success(new VotingResult(false, NODE_IS_NOT_PREVIOUS_CHAIRMAN));
            }
            var validators = await GetValidators();
            if (validators == null || validators.Count == 0)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NO_STAKEHOLDERS_FOUND));
            }
            try
            {
                var (chairman, coucil) = ElectNewEraCouncil(validators, eraId);
                //TODO: propose new era to the blockchain
                var newEraElectionProposal = new ProposeNewEraFunction()
                {
                    EraId_ = eraId.ToByteArray(),
                    Chairman_ = chairman,
                    Coucil_ = coucil.ToList(),
                    UtcTimeStamp_ = TimeStampHelper.GetCurrentUtcTimestamp(),
                    CalculatedSeed_ = _deterministicRandomGenerator.GetCalculatedSeed()
                };

                await _proofOfStakeTokenService.ProposeNewEraElectionAsync(newEraElectionProposal);
                //TODO: Add to db
                //TODO: log in console
                return TryResult<VotingResult>.Success(new VotingResult(true, NEW_ERA_COUNCIL_PROPOSED));
            }
            catch (Exception ex)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<VotingResult>> TryVoteForNewEraElectionAsync(NewEraProposal newEraProposal)
        {
            var nodeData = _nodeDataDataAccessService.GetNodeData();
            if (nodeData == null)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NODE_DATA_NOT_FOUND));
            }

            //TODO; check from DB if such record exist

            if (newEraProposal.Chairman.IsAddressEqualTo(nodeData.ActiveAddress))
            {
                return TryResult<VotingResult>.Success(new VotingResult(true, ALREADY_VOTED));
            }
            //validate!
            try
            {
                var validator = await GetValidators();
                var isEraApproved = ValidateProposedEraElection(newEraProposal, validator);

                var eraProposalVote = new VoteForProposedEraFunction()
                {
                    EraId = newEraProposal.EraId.ToByteArray(),
                    Approves = isEraApproved
                };
                await _proofOfStakeTokenService.VoteForProposedEraRequestAsync(eraProposalVote);
                //TODO: record vote to the database
                //TODO: add log
                return TryResult<VotingResult>.Success(new VotingResult(isEraApproved, NODE_SUCCESSFULLY_VOTE));
            }
            catch (Exception ex)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<bool>> TryEndCurrentEraAsync(string eraId, string chairman)
        {
            var nodeData = _nodeDataDataAccessService.GetNodeData();
            if (nodeData == null)
            {
                return TryResult<bool>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NODE_DATA_NOT_FOUND));
            }

            //TODO: check in the DB if this era is not ended already

            if (chairman != nodeData.ActiveAddress)
            {
                //this node can't end era
                return TryResult<bool>.Success(false);
            }
            try
            {
                var timeStamp = TimeStampHelper.GetCurrentUtcTimestamp();
                await _proofOfStakeTokenService.EndCurrentEraAsync(timeStamp.ToBigInteger());
                //record in the database

                return TryResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return TryResult<bool>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<EraInformation>> GetEraByIdAsync(string eraId)
        {
            var getEraByIdRequest = new GetEraByIdFunction
            {
                EraId_ = eraId.ToByteArray(),
            };

            try
            {
                var result = await _proofOfStakeTokenService.GetEraByIdAsync(getEraByIdRequest);
                var era = result.Era;
                if (era == null || era.Chairman.IsNullAddress())
                {
                    return TryResult<EraInformation>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, ERA_NOT_FOUND));
                }
                var eraInfo = MapToEraIformation(result.Era);

                return TryResult<EraInformation>.Success(eraInfo);
            }
            catch (Exception ex)
            {
                return TryResult<EraInformation>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<EraInformation>> GetCurrentEraAsync()
        {
            try
            {
                var response = await _proofOfStakeTokenService.GetCurrentEraAsync();
                var currentEra = response.CurrentEra;
                if (currentEra == null || currentEra.Chairman.IsNullAddress())
                {
                    return TryResult<EraInformation>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, ERA_NOT_FOUND));
                }
                var eraInfo = MapToEraIformation(currentEra);

                return TryResult<EraInformation>.Success(eraInfo);
            }
            catch (Exception ex)
            {
                return TryResult<EraInformation>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<bool>> RecordEndedEraAsync(string eraId, string chairman, string timeStamp)
        {
            //TODO record in the database
            return TryResult<bool>.Success(true);
        }

        private async Task<ImmutableList<EraElectableMember>> GetValidators()
        {
            //this node is previous chairman and needs to start new election
            var getStakeholders = await _proofOfStakeTokenService.GetStakeholdersAsync();

            if (getStakeholders == null || getStakeholders.Stakeholders == null || getStakeholders.Stakeholders.Count == 0)
            {
                return null;
            }

            var validatorsStakeholder = getStakeholders.Stakeholders.Where(x => x.CanValidate)
                .Select(x => new EraElectableMember(x.User, x.TotalAmount.NormalizeToIntWithDefaultDecimals()))
                .ToImmutableList();

            return validatorsStakeholder;
        }

        private (string, ImmutableList<string>) ElectNewEraCouncil(ImmutableList<EraElectableMember> validators, string eraId, bool shouldInitGenerator = true)
        {
            //0. seed the generator
            if (shouldInitGenerator)
            {
                _deterministicRandomGenerator.Init(eraId.ToByteArray());
            }

            if (validators.Count == 1)
            {
                //one validator - chairman and only memeber of the cocil
                var onlyMember = validators.First();
                return (onlyMember.Address, new List<string>().ToImmutableList());
            }
            //1. calculate total weight
            var totalWeight = validators.Sum(x => x.TotalAmountAsWeight);

            //1. elect chairman
            var firstRandomNumber = _deterministicRandomGenerator.Next(totalWeight);

            var chairman = validators.First(x => (firstRandomNumber -= x.TotalAmountAsWeight) < 0);

            //max amount of coucil member = 10
            //TODO: this should come from blockchain
            var maxConcilMembers = 10;

            if (validators.Count <= maxConcilMembers)
            {
                //all validators must be part of the coucil
                var coucil = validators.Where(x => !x.Address.IsAddressEqualTo(chairman.Address))
                .Select(x => x.Address).ToImmutableList();

                return (chairman.Address, coucil);
            }

            //we must elect 10 unique validators
            //TODO:
            HashSet<string> council = new HashSet<string>();
            while (council.Count < maxConcilMembers)
            {
                var randomNumber = _deterministicRandomGenerator.Next(totalWeight);
                var coucilMember = validators.First(x => (randomNumber -= x.TotalAmountAsWeight) < 0);
                council.Add(coucilMember.Address);
            }

            return (chairman.Address, council.ToImmutableList());
        }

        private bool ValidateProposedEraElection(NewEraProposal newEraProposal, ImmutableList<EraElectableMember> validators)
        {
            _deterministicRandomGenerator.Init(newEraProposal.EraId.ToByteArray());

            var proposedSeed = int.Parse(newEraProposal.CalculatedSeed);

            if (_deterministicRandomGenerator.GetCalculatedSeed() != proposedSeed)
            {
                return false;
            }

            var (excpectedChairman, expectedCouncil) = ElectNewEraCouncil(validators, newEraProposal.EraId, false);

            if (excpectedChairman != newEraProposal.Chairman)
            {
                return false;
            }

            if (expectedCouncil.Count != newEraProposal.Council.Count)
            {
                return false;
            }
            var proposedCoucil = newEraProposal.Council;
            for (int i = 0; i < expectedCouncil.Count; i++)
            {
                if (!expectedCouncil[i].IsAddressEqualTo(proposedCoucil[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private EraInformation MapToEraIformation(Era era)
        {
            return new EraInformation(era.Id.ToHex(),
                    era.ColectedFeesAmount.NormalizeToDefaultDecimal().ToString(),
                    era.StartDate.ToString(),
                    era.EndDate.ToString(),
                    era.Chairman,
                    era.RequiredQuorum.ToString(),
                    era.IsQuorumReached,
                    era.PossitiveVotes.ToString(),
                    era.NegativeVotes.ToString(),
                    era.Accepted,
                    era.Ended);
        }
    }
}