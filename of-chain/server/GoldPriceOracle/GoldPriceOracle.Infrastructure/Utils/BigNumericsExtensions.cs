using Nethereum.Util;
using System;
using System.Numerics;

namespace GoldPriceOracle.Infrastructure.Utils
{
    public static class BigNumericsExtensions
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

        public static int NormalizeToIntWithDefaultDecimals(this BigInteger bigInteger)
        {
            return (int)(bigInteger / BigInteger.Pow(10, 18));
        }

        public static BigInteger ToBitInteger(this string @integer)
        {
            return BigInteger.Parse(@integer);
        }

        public static BigInteger ToBigIntegerWithDefaultDecimals(this decimal value)
        {
            var nonIntegerPart = Math.Truncate(value);
            string franctionalPartAsString = "";
            if (nonIntegerPart < value)
            {
                // we have a decimal part!
                int fractionalPart = new Version(value.ToString()).Minor; //SUPPER HACK !
                franctionalPartAsString = fractionalPart.ToString();
            }

            var truncatedNumbersCount = franctionalPartAsString.Length;
            for (int i = 0; i < truncatedNumbersCount; i++)
            {
                value = value * 10;
            }
            var remainingZeroes = _defaultDecimals - truncatedNumbersCount;

            var result = new BigInteger(value) * BigInteger.Pow(10, remainingZeroes);

            return result;
        }

        public static BigInteger ToBigInteger(this int @iteger)
        {
            return new BigInteger(@iteger);
        }
    }
}