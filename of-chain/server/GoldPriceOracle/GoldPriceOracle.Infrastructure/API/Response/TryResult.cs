namespace GoldPriceOracle.Infrastructure.API.Response
{
    public class TryResult<TResult>
    {
        protected TryResult(TResult result)
        {
            Item = result;
            IsSuccessfull = true;
        }

        protected TryResult(ApiError error)
        {
            Error = error;
            IsSuccessfull = false;
        }

        public TResult Item { get; }

        public ApiError Error { get; }

        public bool IsSuccessfull { get; }

        public static TryResult<TResult> Success(TResult result)
        {
            return new TryResult<TResult>(result);
        }

        public static TryResult<TResult> Fail(ApiError error)
        {
            return new TryResult<TResult>(error);
        }
    }
}