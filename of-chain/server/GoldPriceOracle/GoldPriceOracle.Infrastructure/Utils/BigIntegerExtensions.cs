using Nethereum.Util;
using System.Numerics;

namespace GoldPriceOracle.Infrastructure.Utils
{
    public static class BigIntegerExtensions
    {
        private static short _defaultDecimals = 18;

        /// <summary>
        /// Return "normalized" version of the big intager token balance. Perform the following calculation = amount (as BigInteger) / 10^18 (as BigIntager)
        /// </summary>
        /// <param name="bigInteger"></param>
        /// <returns></returns>
        public static BigDecimal NormalizeToDefaultDecimal(this BigInteger bigInteger)
        {
            BigDecimal bigDecimal = new BigDecimal(bigInteger, 1);
            return (bigDecimal / BigDecimal.Pow(10, 18)) / 10;
        }
    }
}
