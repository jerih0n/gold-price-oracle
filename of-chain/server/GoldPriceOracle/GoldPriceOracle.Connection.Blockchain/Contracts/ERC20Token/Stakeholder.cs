using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace GoldPriceOracle.Connection.Blockchain.ERC20Token
{
    public partial class Stakeholder : StakeholderBase
    { }

    public class StakeholderBase
    {
        [Parameter("address", "user", 1)]
        public virtual string User { get; set; }

        [Parameter("uint256", "totalAmount", 2)]
        public virtual BigInteger TotalAmount { get; set; }

        [Parameter("uint256", "ownedAmount", 3)]
        public virtual BigInteger OwnedAmount { get; set; }

        [Parameter("uint256", "nominatedAmount", 4)]
        public virtual BigInteger NominatedAmount { get; set; }

        [Parameter("uint256", "nominatorsCount", 5)]
        public virtual BigInteger NominatorsCount { get; set; }

        [Parameter("bool", "canValidate", 6)]
        public virtual bool CanValidate { get; set; }

        [Parameter("uint256", "notCollectedTokenRewards", 7)]
        public virtual BigInteger NotCollectedTokenRewards { get; set; }
    }
}