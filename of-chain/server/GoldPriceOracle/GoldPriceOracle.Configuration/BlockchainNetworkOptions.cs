namespace GoldPriceOracle.Configuration
{
    public class BlockchainNetworkOptions
    {
        public string RPCUrl { get; set; }
        public int Port { get; set; }
        public int NetworkId { get; set; }
        public string WebsocketUrl { get; set; }
    }
}