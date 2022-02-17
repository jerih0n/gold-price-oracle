using System.Security.Cryptography;
using System.Text;

namespace GoldPriceOracle.Infrastructure.Cryptography.Password
{
    public static class PasswordCryptoProvider
    {
        private static string Salt = "@#!&@&&@&#&!~DADF@299100:::C";

        public static string EncryptPassword(string plainTextPassword)
        {
            SHA256 Sha256 = SHA256.Create();
            var passwordPattern = $"{Salt}_@4{plainTextPassword}!!caGAGS{plainTextPassword}";
            var passwordByterArray = Encoding.UTF8.GetBytes(passwordPattern);

            var hashedPasswordByterArray = Sha256.ComputeHash(passwordByterArray);

            return Encoding.UTF8.GetString(hashedPasswordByterArray);
        }

        public static bool IsValidPassword(string plainTextPassword, string encryptedPassword)
        {
            var encryptedPassowrdToCheck = EncryptPassword(plainTextPassword);

            return encryptedPassowrdToCheck == encryptedPassword;
        }
    }
}
