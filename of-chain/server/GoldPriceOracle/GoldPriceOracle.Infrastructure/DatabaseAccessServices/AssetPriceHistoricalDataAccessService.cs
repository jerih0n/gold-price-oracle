using GoldPriceOracle.Connection.Database;
using GoldPriceOracle.Connection.Database.Enums;
using System.Linq;

namespace GoldPriceOracle.Infrastructure.DatabaseAccessServices
{
    public class AssetPriceHistoricalDataAccessService : IAssetPriceHistoricalDataAccessService
    {
        private readonly OracleDbContext _oracleDbContext;

        public AssetPriceHistoricalDataAccessService(OracleDbContext oracleDbContext)
        {
            _oracleDbContext = oracleDbContext;
        }

        public bool AddNewHistoricData(AssetHistoricData assetHistoricData)
        {
            _oracleDbContext.Add(assetHistoricData);
            _oracleDbContext.SaveChanges();

            return true;
        }

        public AssetHistoricData GetLatestAssetDataForCurrency(AssetsEnum assetsEnum, CurrenciesEnum currenciesEnum)
        {
            var result = _oracleDbContext.AssetHistoricDatas
                .OrderBy(x => x.TimeStamp)
                .Where(x => x.AssetId == (short)assetsEnum && x.FiatCurrencyId == (short)currenciesEnum)
                .FirstOrDefault();

            return result;
        }
    }
}
