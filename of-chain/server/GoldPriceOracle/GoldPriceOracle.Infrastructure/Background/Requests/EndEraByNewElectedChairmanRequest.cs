namespace GoldPriceOracle.Infrastructure.Background.Requests
{
    public record EndEraByNewElectedChairmanRequest(string EraId, string Chairman, string Timestap);
}