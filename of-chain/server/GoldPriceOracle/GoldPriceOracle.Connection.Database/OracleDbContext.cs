using Microsoft.EntityFrameworkCore;

namespace GoldPriceOracle.Connection.Database
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<NodeData> NodeData { get; set; }
    }
}
