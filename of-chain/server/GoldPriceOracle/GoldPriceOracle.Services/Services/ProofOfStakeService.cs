using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class ProofOfStakeService : IProofOfStakeService
    {
        private readonly IProofOfStakeTokenService _proofOfStakeTokenService;

        public ProofOfStakeService(IProofOfStakeTokenService proofOfStakeTokenService)
        {
            _proofOfStakeTokenService = proofOfStakeTokenService;
        }

        public async Task<TryResult<bool>> TryStartNewEraAsync(string utcTimeStamp)
        {
            throw new NotImplementedException();
        }
    }
}