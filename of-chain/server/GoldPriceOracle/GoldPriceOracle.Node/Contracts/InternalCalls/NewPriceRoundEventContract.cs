namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public class NewPriceRoundEventContract
    {
        public string AssetSymbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string RoundId { get; set; }
        public string ProposalEmiterAddress { get; set; }
        public string Price { get; set; }
    }
}