using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Services.Interfaces;
using System;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class StakingManagerService : BaseAuthorizedService, IStakingManagerService
    {
        private readonly IGoldPriceOracleERC20TokenService _goldPriceOracleERC20TokenService;

        public StakingManagerService(IGoldPriceOracleERC20TokenService goldPriceOracleERC20TokenService,
            INodeDataDataAccessService nodeDataDataAccessService) :
            base(nodeDataDataAccessService)
        {
            _goldPriceOracleERC20TokenService = goldPriceOracleERC20TokenService;
        }

        public async Task<TryResult<bool>> StakeAmountAsync(string password, decimal amount)
        {
            try
            {
                var autorizeResult = Authorize(password);
                if (!autorizeResult.Item1)
                {
                    return TryResult<bool>.Fail(autorizeResult.Item2);
                }

                var nodeData = autorizeResult.Item3;

                var dataAsBigIntager = amount.ToBigIntegerWithDefaultDecimals();

                await _goldPriceOracleERC20TokenService.StakeAmountAsync(dataAsBigIntager);

                return TryResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return TryResult<bool>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<TryResult<bool>> UnstakeAmountAsync(string password, decimal amount)
        {
            try
            {
                var autorizeResult = Authorize(password);
                if (!autorizeResult.Item1)
                {
                    return TryResult<bool>.Fail(autorizeResult.Item2);
                }

                var nodeData = autorizeResult.Item3;

                var dataAsBigIntager = amount.ToBigIntegerWithDefaultDecimals();

                await _goldPriceOracleERC20TokenService.UnstakeAmountAsync(dataAsBigIntager);

                return TryResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return TryResult<bool>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}