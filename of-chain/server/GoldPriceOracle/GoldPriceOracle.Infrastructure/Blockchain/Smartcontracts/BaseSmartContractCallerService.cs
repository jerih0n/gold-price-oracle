using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Database;
using GoldPriceOracle.Infrastructure.Cryptography.AES;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts
{
    public abstract class BaseSmartContractCallerService : IBaseSmartContractCallerService
    {
        private readonly BlockchainNetworkOptions _blockchainNetworkOptions;
        private readonly OracleDbContext _oracleDbContext;
        public BaseSmartContractCallerService(IOptionsMonitor<BlockchainNetworkOptions> optionsMonitor, OracleDbContext oracleDbContext)
        {
            _oracleDbContext = oracleDbContext;
            _blockchainNetworkOptions = optionsMonitor.CurrentValue;

            var nodeData = GetNodeData();

            if (nodeData == null)
            {
                Web3 = new Web3($"{_blockchainNetworkOptions.RPCUrl}:{_blockchainNetworkOptions.Port}");
            }
            else
            {
                var chainId = _blockchainNetworkOptions.NetworkId;
                var privateKey = AESCryptoProvider.Decrypt(nodeData.PrivateKeyEncrypted, nodeData.Password);
                Account account = new Account(privateKey, new BigInteger(chainId));
                Web3 = new Web3(account, $"{_blockchainNetworkOptions.RPCUrl}:{_blockchainNetworkOptions.Port}");
                Web3.TransactionManager.UseLegacyAsDefault = true;
            }
        }

        protected Web3 Web3 { get; }

        public async Task<BigInteger> GetEthAmountAsync(string address)
            => await Web3.Eth.GetBalance.SendRequestAsync(address);

        protected NodeData GetNodeData() => _oracleDbContext.NodeData.FirstOrDefault();

    }
}
