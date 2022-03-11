namespace GoldPriceOracle.Infrastructure.Background.Requests
{
    public class NewEraElectionComplitedEventRequest
    {
        public NewEraElectionComplitedEventRequest(EraRequest eraRequest)
        {
            Era = eraRequest;
        }

        public EraRequest Era { get; }
    }
}