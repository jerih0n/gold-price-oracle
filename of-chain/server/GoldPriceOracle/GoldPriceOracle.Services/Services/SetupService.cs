using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Services.Interfaces;

namespace GoldPriceOracle.Services.Services
{
    public class SetupService : ISetupService
    {
        private readonly INodeDataDataAccessService _nodeDataAccessService;

        public SetupService(INodeDataDataAccessService nodeDataDataAccessService)
        {
            _nodeDataAccessService = nodeDataDataAccessService;
        }

        public bool IsNodeSetup()
        {
            var nodeData = _nodeDataAccessService.GetNodeData();
            return nodeData != null;
        }

        public bool SetupNode(string password, string privateKey, bool isInMnemonicFormat)
        {
            return isInMnemonicFormat ? SetupNodeWithMnemonic(password, privateKey) : SetupNodeWithPrivateKey(password, privateKey);
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
