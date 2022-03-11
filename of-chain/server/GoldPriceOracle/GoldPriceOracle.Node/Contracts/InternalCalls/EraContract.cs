namespace GoldPriceOracle.Node.Contracts.InternalCalls
{
    public record EraContract(string Id,
       string ColectedFeesAmount,
       string StartDate,
       string EndDate,
       string Chairman,
       string RequiredQuorum,
       bool IsQuorumReached,
       string PossitiveVotes,
       string NegativeVotes,
       bool Accepted,
       bool Ended);
}