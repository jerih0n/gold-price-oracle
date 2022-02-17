namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class SetupNodeContract
    {
        public SetupNodeContract(string password)
        {
            Password = password;
        }
        public string Password { get;  }
    }
}
