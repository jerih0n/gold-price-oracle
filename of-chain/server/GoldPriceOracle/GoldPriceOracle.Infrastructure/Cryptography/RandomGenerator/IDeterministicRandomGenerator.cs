namespace GoldPriceOracle.Infrastructure.Cryptography.RandomGenerator
{
    public interface IDeterministicRandomGenerator
    {
        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        void Init(byte[] seedFromRoundId);

        void Reset();

        int GetCalculatedSeed();
    }
}