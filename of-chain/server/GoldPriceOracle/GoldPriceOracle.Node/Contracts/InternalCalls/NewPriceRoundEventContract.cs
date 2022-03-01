using System.Numerics;

namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public class NewPriceRoundEventContract
    {
        public string AssetSymbol { get; set; }

        public string CurrencySymbol { get; set; }

        public byte[] RoundId { get; set; }

        public string ProposalEmiterAddress { get; set; }

        public BigInteger Price { get; set; }
    }
}
