namespace GoldPriceOracle.Services.Models.Information
{
    public class OracleTokenBalance
    {
        public OracleTokenBalance(string tokenBalance, string tokenSymbol)
        {
            TokenBalance = tokenBalance;
            TokenSymbol = tokenSymbol;
        }

        public string TokenBalance { get; set; }
        public string TokenSymbol { get; set; }
    }
}
