namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public record NewPriceRoundEventContract(string AssetSymbol, string CurrencySymbol, string RoundId, string ProposalEmiterAddress, string Price);
}