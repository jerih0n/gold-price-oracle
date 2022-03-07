namespace GoldPriceOracle.Infrastructure.Background.Requests
{
    public record NewPriceRoundVoteRequest(string AssetSymbol, string CurrencySymbol, string RoundId, string ProposalEmiterAddress, string Price);
}