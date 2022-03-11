using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace GoldPriceOracle.Connection.Blockchain.ERC20Token
{
    public partial class Era : EraBase
    { }

    public class EraBase
    {
        [Parameter("bytes32", "id", 1)]
        public virtual byte[] Id { get; set; }

        [Parameter("uint256", "colectedFeesAmount", 2)]
        public virtual BigInteger ColectedFeesAmount { get; set; }

        [Parameter("uint256", "startDate", 3)]
        public virtual BigInteger StartDate { get; set; }

        [Parameter("uint256", "endDate", 4)]
        public virtual BigInteger EndDate { get; set; }

        [Parameter("address", "chairman", 5)]
        public virtual string Chairman { get; set; }

        [Parameter("uint256", "requiredQuorum", 6)]
        public virtual BigInteger RequiredQuorum { get; set; }

        [Parameter("bool", "isQuorumReached", 7)]
        public virtual bool IsQuorumReached { get; set; }

        [Parameter("uint256", "possitiveVotes", 8)]
        public virtual BigInteger PossitiveVotes { get; set; }

        [Parameter("uint256", "negativeVotes", 9)]
        public virtual BigInteger NegativeVotes { get; set; }

        [Parameter("bool", "accepted", 10)]
        public virtual bool Accepted { get; set; }

        [Parameter("bool", "ended", 11)]
        public virtual bool Ended { get; set; }
    }
}