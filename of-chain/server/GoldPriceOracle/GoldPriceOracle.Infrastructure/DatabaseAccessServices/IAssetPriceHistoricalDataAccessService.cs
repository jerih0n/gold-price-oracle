using GoldPriceOracle.Connection.Database;
using GoldPriceOracle.Connection.Database.Enums;

namespace GoldPriceOracle.Infrastructure.DatabaseAccessServices
{
    public interface IAssetPriceHistoricalDataAccessService
    {
        AssetHistoricData GetLatestAssetDataForCurrency(AssetsEnum assetsEnum, CurrenciesEnum currenciesEnum);

        bool AddNewHistoricData(AssetHistoricData assetHistoricData);
    }
}