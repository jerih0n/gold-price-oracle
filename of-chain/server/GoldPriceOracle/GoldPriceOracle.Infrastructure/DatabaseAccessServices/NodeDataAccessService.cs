using GoldPriceOracle.Connection.Database;
using System;
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

        public bool CreateNewNode(string password, string privateKey, string activeAddress, string mnemonic)
        {
            try
            {
                _dbContext.NodeData.Add(new NodeData
                {
                    Password = password,
                    PrivateKeyEncrypted = privateKey,
                    ActiveAddress = activeAddress,
                    MnemonicPhraseEncrypted = mnemonic
                });

                _dbContext.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public NodeData GetNodeData()
        {
            return _dbContext.NodeData.FirstOrDefault();
        }
    }
}
