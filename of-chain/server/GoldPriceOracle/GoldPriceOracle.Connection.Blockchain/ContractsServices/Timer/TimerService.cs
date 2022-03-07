using System.Threading.Tasks;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using System.Threading;
using GoldPriceOracle.Connection.Blockchain.Contracts.Timer;

namespace GoldPriceOracle.Connection.Blockchain.ContractsServices.Timer
{
    public partial class TimerService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TimerDeployment timerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TimerDeployment>().SendRequestAndWaitForReceiptAsync(timerDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TimerDeployment timerDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TimerDeployment>().SendRequestAsync(timerDeployment);
        }

        public static async Task<TimerService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TimerDeployment timerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, timerDeployment, cancellationTokenSource);
            return new TimerService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public TimerService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> NewEraTimeElapsedRequestAsync(NewEraTimeElapsedFunction newEraTimeElapsedFunction)
        {
            return ContractHandler.SendRequestAsync(newEraTimeElapsedFunction);
        }

        public Task<TransactionReceipt> NewEraTimeElapsedRequestAndWaitForReceiptAsync(NewEraTimeElapsedFunction newEraTimeElapsedFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(newEraTimeElapsedFunction, cancellationToken);
        }

        public Task<string> NewEraTimeElapsedRequestAsync(BigInteger utcTimeStamp_)
        {
            var newEraTimeElapsedFunction = new NewEraTimeElapsedFunction();
            newEraTimeElapsedFunction.UtcTimeStamp_ = utcTimeStamp_;

            return ContractHandler.SendRequestAsync(newEraTimeElapsedFunction);
        }

        public Task<TransactionReceipt> NewEraTimeElapsedRequestAndWaitForReceiptAsync(BigInteger utcTimeStamp_, CancellationTokenSource cancellationToken = null)
        {
            var newEraTimeElapsedFunction = new NewEraTimeElapsedFunction();
            newEraTimeElapsedFunction.UtcTimeStamp_ = utcTimeStamp_;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(newEraTimeElapsedFunction, cancellationToken);
        }

        public Task<string> NewPriceRoundTimeElapsedRequestAsync(NewPriceRoundTimeElapsedFunction newPriceRoundTimeElapsedFunction)
        {
            return ContractHandler.SendRequestAsync(newPriceRoundTimeElapsedFunction);
        }

        public Task<TransactionReceipt> NewPriceRoundTimeElapsedRequestAndWaitForReceiptAsync(NewPriceRoundTimeElapsedFunction newPriceRoundTimeElapsedFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(newPriceRoundTimeElapsedFunction, cancellationToken);
        }

        public Task<string> NewPriceRoundTimeElapsedRequestAsync(BigInteger utcTimeStamp_)
        {
            var newPriceRoundTimeElapsedFunction = new NewPriceRoundTimeElapsedFunction();
            newPriceRoundTimeElapsedFunction.UtcTimeStamp_ = utcTimeStamp_;

            return ContractHandler.SendRequestAsync(newPriceRoundTimeElapsedFunction);
        }

        public Task<TransactionReceipt> NewPriceRoundTimeElapsedRequestAndWaitForReceiptAsync(BigInteger utcTimeStamp_, CancellationTokenSource cancellationToken = null)
        {
            var newPriceRoundTimeElapsedFunction = new NewPriceRoundTimeElapsedFunction();
            newPriceRoundTimeElapsedFunction.UtcTimeStamp_ = utcTimeStamp_;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(newPriceRoundTimeElapsedFunction, cancellationToken);
        }
    }
}