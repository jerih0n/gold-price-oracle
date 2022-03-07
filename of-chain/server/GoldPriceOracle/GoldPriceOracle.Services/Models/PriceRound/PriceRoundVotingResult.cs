namespace GoldPriceOracle.Services.Models.PriceRound
{
    public class PriceRoundVotingResult
    {
        public PriceRoundVotingResult(bool successfullVote, string message)
        {
            SuccessfullVote = successfullVote;
            Message = message;
        }

        private bool SuccessfullVote { get; }
        private string Message { get; }
    }
}