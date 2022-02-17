using GoldPriceOracle.Infrastructure.Blockchain.Accounts;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Services.Interfaces;

namespace GoldPriceOracle.Services.Services
{
    public class SetupService : ISetupService
    {
        private readonly INodeDataDataAccessService _nodeDataAccessService;
        private readonly IHDWalletManagingService _HDWalletManagingService;

        public SetupService(INodeDataDataAccessService nodeDataDataAccessService, IHDWalletManagingService hDWalletManagingService)
        {
            _nodeDataAccessService = nodeDataDataAccessService;
            _HDWalletManagingService = hDWalletManagingService;
        }

        public bool IsNodeSetup()
        {
            var nodeData = _nodeDataAccessService.GetNodeData();
            return nodeData != null;
        }

        public bool SetupNode(string password)
        {
            var account = _HDWalletManagingService.CreateNewWallet();
            var address = account.Address;
            var privateKey = account.PrivateKey;

            //TODO: needs to be fixed
            return _nodeDataAccessService.CreateNewNode(password, privateKey, address);
        }

        private bool SetupNodeWithMnemonic(string password, string mnemonic)
        {
            //TODO:
            return true;
        }

        private bool SetupNodeWithPrivateKey(string password, string mnemonic)
        {
            //TODO:
            return true;
        }
    }
}
