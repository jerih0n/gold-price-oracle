using GoldPriceOracle.Connection.Database;

namespace GoldPriceOracle.Infrastructure.DatabaseAccessServices
{
    public interface INodeDataDataAccessService
    {
        NodeData GetNodeData();

        bool CreateNewNode(string password, string privateKey, string activeAddress, string mnemonic);
    }
}