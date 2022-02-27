using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace GoldPriceOracle.Connection.Blockchain.Contracts.GoldPriceResolver
{
    public partial class Round : RoundBase { }

    public class RoundBase 
    {
        [Parameter("bytes32", "roundId", 1)]
        public virtual byte[] RoundId { get; set; }
        [Parameter("uint256", "nonce", 2)]
        public virtual BigInteger Nonce { get; set; }
        [Parameter("address", "node", 3)]
        public virtual string Node { get; set; }
        [Parameter("uint256", "price", 4)]
        public virtual BigInteger Price { get; set; }
        [Parameter("string", "assetCode", 5)]
        public virtual string AssetCode { get; set; }
        [Parameter("string", "currencyCode", 6)]
        public virtual string CurrencyCode { get; set; }
        [Parameter("uint256", "requiredQuorum", 7)]
        public virtual BigInteger RequiredQuorum { get; set; }
        [Parameter("bool", "isQuorumReached", 8)]
        public virtual bool IsQuorumReached { get; set; }
        [Parameter("uint256", "acceptVotes", 9)]
        public virtual BigInteger AcceptVotes { get; set; }
        [Parameter("uint256", "refuseVotes", 10)]
        public virtual BigInteger RefuseVotes { get; set; }
    }
}
