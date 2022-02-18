using Microsoft.EntityFrameworkCore;

namespace GoldPriceOracle.Connection.Database
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<NodeData> NodeData { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<FiatCurrency> FiatCurrencies { get; set; }
        public DbSet<AssetHistoricData> AssetHistoricDatas { get; set; }
    }
}
