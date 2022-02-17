using GoldPriceOracle.Connection.Database;
using System.Linq;

namespace GoldPriceOracle.Infrastructure.DatabaseAccessServices
{
    public class NodeDataAccessService : INodeDataDataAccessService
    {
        private readonly OracleDbContext _dbContext;
        public NodeDataAccessService(OracleDbContext oracleDbContext)
        {
            _dbContext = oracleDbContext;
        }

        public NodeData GetNodeData()
        {
            return _dbContext.NodeData.FirstOrDefault();
        }
    }
}
