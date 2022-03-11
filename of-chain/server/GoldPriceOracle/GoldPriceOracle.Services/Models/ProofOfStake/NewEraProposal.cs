using System.Collections.Generic;

namespace GoldPriceOracle.Services.Models.ProofOfStake
{
    public class NewEraProposal
    {
        public NewEraProposal(string eraId, string chairman, List<string> council, string validatorsCount, string calculatedSeed)
        {
            EraId = eraId;
            Chairman = chairman;
            Council = council;
            ValidatorsCount = validatorsCount;
            CalculatedSeed = calculatedSeed;
        }

        public virtual string EraId { get; }
        public virtual string Chairman { get; }
        public virtual List<string> Council { get; }
        public virtual string ValidatorsCount { get; }
        public virtual string CalculatedSeed { get; }
    }
}