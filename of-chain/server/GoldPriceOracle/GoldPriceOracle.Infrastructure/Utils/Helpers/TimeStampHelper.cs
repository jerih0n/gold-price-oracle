using System;

namespace GoldPriceOracle.Infrastructure.Utils.Helpers
{
    public static class TimeStampHelper
    {
        public static int GetCurrentUtcTimestamp()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}