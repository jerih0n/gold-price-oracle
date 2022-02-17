using GoldPriceOracle.Connection.Database;

namespace GoldPriceOracle.Infrastructure.DatabaseAccessServices
{
    public interface INodeDataDataAccessService
    {
        NodeData GetNodeData();
    }
}
