namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public class NewEraElectionComplitedContract
    {
        public NewEraElectionComplitedContract(EraContract eraRequest)
        {
            Era = eraRequest;
        }

        public EraContract Era { get; }
    }
}