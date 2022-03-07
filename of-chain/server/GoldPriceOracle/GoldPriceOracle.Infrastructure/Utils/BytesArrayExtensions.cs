using System;
using System.Collections.Generic;
using System.Text;

namespace GoldPriceOracle.Infrastructure.Utils
{
    public static class BytesArrayExtensions
    {
        public static string ToHex(this byte[] key)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var oneByteOfKey in key)
            {
                builder.Append(BitConverter.ToString(new byte[] { oneByteOfKey }));
            }

            var privateKeyAsHex = builder.ToString();
            return privateKeyAsHex;
        }

        public static byte[] ToByteArray(this string keyAsHex)
        {
            var keyLenght = keyAsHex.Length;
            if (keyLenght % 2 != 0)
            {
                //invalid format
                throw new Exception("Invalid key passed");
            }
            List<byte> result = new List<byte>(keyLenght / 2);
            for (int i = 1; i < keyLenght; i += 2)
            {
                var resultByte = Convert.ToByte($"{keyAsHex[i - 1]}{keyAsHex[i]}", 16);
                result.Add(resultByte);
            }

            return result.ToArray();
        }
    }
}