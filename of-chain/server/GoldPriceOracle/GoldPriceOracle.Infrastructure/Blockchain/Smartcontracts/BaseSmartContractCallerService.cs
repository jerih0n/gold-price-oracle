using GoldPriceOracle.Configuration;
using Microsoft.Extensions.Options;
using Nethereum.Web3;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts
{
    public abstract class BaseSmartContractCallerService
    {
        private readonly BlockchainNetworkOptions _blockchainNetworkOptions;

        public BaseSmartContractCallerService(IOptionsMonitor<BlockchainNetworkOptions> optionsMonitor)
        {
            _blockchainNetworkOptions = optionsMonitor.CurrentValue;
            Web3 = new Web3($"{_blockchainNetworkOptions.RPCUrl}:{_blockchainNetworkOptions.Port}");

        }

        protected Web3 Web3 { get; }
    }
}
