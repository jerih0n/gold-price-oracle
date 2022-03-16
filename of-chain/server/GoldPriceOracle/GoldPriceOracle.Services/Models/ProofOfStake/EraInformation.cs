namespace GoldPriceOracle.Services.Models.ProofOfStake
{
    public record EraInformation(string Id,
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