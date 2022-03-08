using GoldPriceOracle.Connection.Database.Enums;
using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.GoldPriceResolver;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.Voting;
using System;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class PriceRoundService : IPriceRoundService
    {
        private const string NODE_DATA_NOT_SET_ERROR_MESSAGE = "Node not set. Node must be set before participating";
        private const string NODE_IS_ROUND_PROPOSAL_ERROR_MESSAGE = "This node is the current round price proposal. Cannot vote twice";
        private const string NODE_VOTED_SUCCESSFULLY_MESSAGE = "Node has voted successfully";

        private readonly IGoldPriceResolverSmartcontractService _goldPriceResolverService;
        private readonly INodeDataDataAccessService _nodeDataDataAccessService;
        private readonly IIntegrationService _integrationService;

        public PriceRoundService(IGoldPriceResolverSmartcontractService goldPriceResolverService,
            INodeDataDataAccessService nodeDataDataAccessService,
            IIntegrationService integrationService)
        {
            _goldPriceResolverService = goldPriceResolverService;
            _nodeDataDataAccessService = nodeDataDataAccessService;
            _integrationService = integrationService;
        }

        public async Task<TryResult<VotingResult>> TryVoteForNewPriceRoundAsync(string roundId,
            string priceAsBigNumber,
            string proposalAddress,
            string assetCode,
            string currencyCode)
        {
            try
            {
                var nodeData = _nodeDataDataAccessService.GetNodeData();
                if (nodeData == null)
                {
                    return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.NotFound, NODE_DATA_NOT_SET_ERROR_MESSAGE));
                }

                var address = nodeData.ActiveAddress;
                if (proposalAddress.ToLower() == address.ToLower())
                {
                    var result = new VotingResult(false, NODE_IS_ROUND_PROPOSAL_ERROR_MESSAGE);

                    return TryResult<VotingResult>.Success(result);
                }

                var response = await _integrationService.GetAssetPriceModelAsync(assetCode.ToUpper(), CurrenciesEnum.USD.ToString().ToUpper());

                if (!response.IsSuccessfull)
                {
                    return TryResult<VotingResult>.Fail(response.Error);
                }

                var priceAsBigInteger = response.Item.Price.ToBigIntegerWithDefaultDecimals();

                await _goldPriceResolverService.TryVoteForPriceRoundAsync(roundId.ToByteArray(), priceAsBigInteger);

                // TODO: record all the activity into the database
                return TryResult<VotingResult>.Success(new VotingResult(true, NODE_VOTED_SUCCESSFULLY_MESSAGE));
            }
            catch (Exception ex)
            {
                return TryResult<VotingResult>.Fail(new ApiError(System.Net.HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}