namespace GoldPriceOracle.Infrastructure.Utils
{
    public static class AddressExtensions
    {
        private const string NULL_ADDRESS = "0x0000000000000000000000000000000000000000";

        public static bool IsNullAddress(this string address)
        {
            return address.ToUpper() == NULL_ADDRESS;
        }

        public static bool IsAddressEqualTo(this string address, string compareAddress)
        {
            return address.ToUpper() == compareAddress.ToUpper();
        }
    }
}