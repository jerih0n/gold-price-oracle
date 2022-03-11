using System.Collections.Generic;

namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public class NewEraProposalEventContract
    {
        public virtual string EraId { get; set; }
        public virtual string Chairman { get; set; }
        public virtual List<string> Council { get; set; }
        public virtual string ValidatorsCount { get; set; }
        public virtual string CalculatedSeed { get; set; }
    }
}