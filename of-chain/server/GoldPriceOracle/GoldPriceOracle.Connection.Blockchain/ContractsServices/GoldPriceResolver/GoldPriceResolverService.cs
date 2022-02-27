using System.Threading.Tasks;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using System.Threading;
using GoldPriceOracle.Connection.Blockchain.Contracts.GoldPriceResolver;

namespace GoldPriceOracle.Connection.Blockchain.ContractsServices.GoldPriceResolver
{
    public partial class GoldPriceResolverService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, GoldPriceResolverDeployment goldPriceResolverDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<GoldPriceResolverDeployment>().SendRequestAndWaitForReceiptAsync(goldPriceResolverDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, GoldPriceResolverDeployment goldPriceResolverDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<GoldPriceResolverDeployment>().SendRequestAsync(goldPriceResolverDeployment);
        }

        public static async Task<GoldPriceResolverService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, GoldPriceResolverDeployment goldPriceResolverDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, goldPriceResolverDeployment, cancellationTokenSource);
            return new GoldPriceResolverService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public GoldPriceResolverService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<GetLatestValidDataOutputDTO> GetLatestValidDataQueryAsync(GetLatestValidDataFunction getLatestValidDataFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetLatestValidDataFunction, GetLatestValidDataOutputDTO>(getLatestValidDataFunction, blockParameter);
        }

        public Task<GetLatestValidDataOutputDTO> GetLatestValidDataQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetLatestValidDataFunction, GetLatestValidDataOutputDTO>(null, blockParameter);
        }

        public Task<string> StartNewPriceRoundRequestAsync(StartNewPriceRoundFunction startNewPriceRoundFunction)
        {
             return ContractHandler.SendRequestAsync(startNewPriceRoundFunction);
        }

        public Task<TransactionReceipt> StartNewPriceRoundRequestAndWaitForReceiptAsync(StartNewPriceRoundFunction startNewPriceRoundFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startNewPriceRoundFunction, cancellationToken);
        }

        public Task<string> StartNewPriceRoundRequestAsync(string nodeAddress_, BigInteger price_)
        {
            var startNewPriceRoundFunction = new StartNewPriceRoundFunction();
                startNewPriceRoundFunction.NodeAddress_ = nodeAddress_;
                startNewPriceRoundFunction.Price_ = price_;
            
             return ContractHandler.SendRequestAsync(startNewPriceRoundFunction);
        }

        public Task<TransactionReceipt> StartNewPriceRoundRequestAndWaitForReceiptAsync(string nodeAddress_, BigInteger price_, CancellationTokenSource cancellationToken = null)
        {
            var startNewPriceRoundFunction = new StartNewPriceRoundFunction();
                startNewPriceRoundFunction.NodeAddress_ = nodeAddress_;
                startNewPriceRoundFunction.Price_ = price_;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startNewPriceRoundFunction, cancellationToken);
        }

        public Task<string> VotePriceForRoundRequestAsync(VotePriceForRoundFunction votePriceForRoundFunction)
        {
             return ContractHandler.SendRequestAsync(votePriceForRoundFunction);
        }

        public Task<TransactionReceipt> VotePriceForRoundRequestAndWaitForReceiptAsync(VotePriceForRoundFunction votePriceForRoundFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(votePriceForRoundFunction, cancellationToken);
        }

        public Task<string> VotePriceForRoundRequestAsync(byte[] roundId_, BigInteger price_)
        {
            var votePriceForRoundFunction = new VotePriceForRoundFunction();
                votePriceForRoundFunction.RoundId_ = roundId_;
                votePriceForRoundFunction.Price_ = price_;
            
             return ContractHandler.SendRequestAsync(votePriceForRoundFunction);
        }

        public Task<TransactionReceipt> VotePriceForRoundRequestAndWaitForReceiptAsync(byte[] roundId_, BigInteger price_, CancellationTokenSource cancellationToken = null)
        {
            var votePriceForRoundFunction = new VotePriceForRoundFunction();
                votePriceForRoundFunction.RoundId_ = roundId_;
                votePriceForRoundFunction.Price_ = price_;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(votePriceForRoundFunction, cancellationToken);
        }
    }
}
