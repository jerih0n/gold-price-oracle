namespace GoldPriceOracle.Infrastructure.Background.Requests
{
    public class NewPriceRoundRequest
    {
        public string AssetSymbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string RoundId { get; set; }
        public string ProposalEmiterAddress { get; set; }
        public string Price { get; set; }
    }
}
