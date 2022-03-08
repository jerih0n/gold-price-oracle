using System;

namespace GoldPriceOracle.Infrastructure.Cryptography.RandomGenerator
{
    //This is very simple deterministic random generator. It seeds the generator with fixed fraction of the 256 bit era id;
    public class SimpleDeterministicRandomGenerator : IDeterministicRandomGenerator
    {
        private const string INVALID_SEED_PASSED = "Invalid seed passed. Must be 256 bit represented as 32 bytes array";
        private const string RANDOM_GENERATOR_ALREADY_INITED = "Random generator is aldready inited";
        private const string RANDOM_GENERATOR_NOT_BEEN_INITED = "Random generator not been inited";
        private Random _random;

        private int _calculatedSeed;
        private bool isInited = false;

        public int Next(int maxValue)
        {
            if (!isInited) throw new Exception(RANDOM_GENERATOR_NOT_BEEN_INITED);

            return _random.Next(maxValue);
        }

        public void Init(byte[] seedFromRoundId)
        {
            if (isInited) throw new Exception(RANDOM_GENERATOR_ALREADY_INITED);

            (_random, _calculatedSeed) = SeedGenerator(seedFromRoundId);
            isInited = true;
        }

        public int Next(int minValue, int maxValue)
        {
            if (!isInited) throw new Exception(RANDOM_GENERATOR_NOT_BEEN_INITED);

            return _random.Next(minValue, maxValue);
        }

        public int GetCalculatedSeed()
        {
            return _calculatedSeed;
        }

        private (Random, int) SeedGenerator(byte[] seedFromRoundId)
        {
            if (seedFromRoundId.Length != 32)
            {
                throw new Exception($"{INVALID_SEED_PASSED}. Got {seedFromRoundId.Length} bytes array");
            }
            int seed = 0;

            for (short i = 0; i < seedFromRoundId.Length; i++)
            {
                seed += seedFromRoundId[i];
            }

            var random = new Random(seed);

            return (random, seed);
        }

        public void Reset()
        {
            isInited = false;
            _random = null;
        }
    }
}