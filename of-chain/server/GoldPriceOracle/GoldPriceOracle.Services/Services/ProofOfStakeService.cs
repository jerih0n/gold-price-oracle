﻿using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Infrastructure.Cryptography.RandomGenerator;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Utils;
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
    public class ProofOfStakeService : IProofOfStakeService
    {
        private const string CURRENT_ERA_NOT_FOUND = "Current era not found";
        private const string NODE_DATA_NOT_FOUND = "Node is not set up. You need to set the node";
        private const string NODE_IS_NOT_PREVIOUS_CHAIRMAN = "Node is not previous chairman. Cannot propose new election";
        private const string NO_STAKEHOLDERS_FOUND = "No validator stakeholders found";

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
                || currentEra.CurrentEra.Chairman == null
                || currentEra.CurrentEra.Chairman.IsNullAddress())
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, CURRENT_ERA_NOT_FOUND));
            }
            var nodeData = _nodeDataDataAccessService.GetNodeData();

            if (nodeData == null)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NODE_DATA_NOT_FOUND));
            }
            if (!nodeData.ActiveAddress.IsAddressEqualTo(currentEra.CurrentEra.Chairman))
            {
                return TryResult<VotingResult>.Success(new VotingResult(false, NODE_IS_NOT_PREVIOUS_CHAIRMAN));
            }
            var validators = await GetValidators();
            if (validators == null || validators.Count == 0)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NO_STAKEHOLDERS_FOUND));
            }
            throw new NotImplementedException();
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

        private (string, ImmutableList<string>) ElectNewEraCouncil(ImmutableList<EraElectableMember> validators, string eraId)
        {
            if (validators.Count == 1)
            {
                //one validator - chairman and only memeber of the cocil
                var onlyMember = validators.First();
                return (onlyMember.Address, new List<string>().ToImmutableList());
            }
            //0. calculate total weight
            var totalWeight = validators.Sum(x => x.TotalAmountAsWeight);

            //1. seed the generator
            _deterministicRandomGenerator.Init(eraId.ToByteArray());

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
            HashSet<string> concil = new HashSet<string>();
            throw new Exception();
        }
    }
}