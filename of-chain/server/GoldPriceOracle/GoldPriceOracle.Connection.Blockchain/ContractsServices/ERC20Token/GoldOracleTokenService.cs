using System.Threading.Tasks;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using System.Threading;
using GoldPriceOracle.Connection.Blockchain.ERC20Token;

namespace GoldPriceOracle.Connection.Blockchain.ContractsServices.ERC20Token
{
    public partial class GoldOracleTokenService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, GoldOracleTokenDeployment goldOracleTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<GoldOracleTokenDeployment>().SendRequestAndWaitForReceiptAsync(goldOracleTokenDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, GoldOracleTokenDeployment goldOracleTokenDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<GoldOracleTokenDeployment>().SendRequestAsync(goldOracleTokenDeployment);
        }

        public static async Task<GoldOracleTokenService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, GoldOracleTokenDeployment goldOracleTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, goldOracleTokenDeployment, cancellationTokenSource);
            return new GoldOracleTokenService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public GoldOracleTokenService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
            allowanceFunction.Owner = owner;
            allowanceFunction.Spender = spender;

            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
            return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
            approveFunction.Spender = spender;
            approveFunction.Amount = amount;

            return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
            approveFunction.Spender = spender;
            approveFunction.Amount = amount;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Account = account;

            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<string> DecreaseAllowanceRequestAsync(DecreaseAllowanceFunction decreaseAllowanceFunction)
        {
            return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(DecreaseAllowanceFunction decreaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DecreaseAllowanceRequestAsync(string spender, BigInteger subtractedValue)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
            decreaseAllowanceFunction.Spender = spender;
            decreaseAllowanceFunction.SubtractedValue = subtractedValue;

            return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger subtractedValue, CancellationTokenSource cancellationToken = null)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
            decreaseAllowanceFunction.Spender = spender;
            decreaseAllowanceFunction.SubtractedValue = subtractedValue;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> ElectNewChairmanRequestAsync(ElectNewChairmanFunction electNewChairmanFunction)
        {
            return ContractHandler.SendRequestAsync(electNewChairmanFunction);
        }

        public Task<string> ElectNewChairmanRequestAsync()
        {
            return ContractHandler.SendRequestAsync<ElectNewChairmanFunction>();
        }

        public Task<TransactionReceipt> ElectNewChairmanRequestAndWaitForReceiptAsync(ElectNewChairmanFunction electNewChairmanFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(electNewChairmanFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ElectNewChairmanRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<ElectNewChairmanFunction>(null, cancellationToken);
        }

        public Task<string> EndCurrentEraRequestAsync(EndCurrentEraFunction endCurrentEraFunction)
        {
            return ContractHandler.SendRequestAsync(endCurrentEraFunction);
        }

        public Task<string> EndCurrentEraRequestAsync()
        {
            return ContractHandler.SendRequestAsync<EndCurrentEraFunction>();
        }

        public Task<TransactionReceipt> EndCurrentEraRequestAndWaitForReceiptAsync(EndCurrentEraFunction endCurrentEraFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(endCurrentEraFunction, cancellationToken);
        }

        public Task<TransactionReceipt> EndCurrentEraRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<EndCurrentEraFunction>(null, cancellationToken);
        }

        public Task<GetCurrentEraOutputDTO> GetCurrentEraQueryAsync(GetCurrentEraFunction getCurrentEraFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetCurrentEraFunction, GetCurrentEraOutputDTO>(getCurrentEraFunction, blockParameter);
        }

        public Task<GetCurrentEraOutputDTO> GetCurrentEraQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetCurrentEraFunction, GetCurrentEraOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetErasCountQueryAsync(GetErasCountFunction getErasCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetErasCountFunction, BigInteger>(getErasCountFunction, blockParameter);
        }

        public Task<BigInteger> GetErasCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetErasCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GetOwnerAddressQueryAsync(GetOwnerAddressFunction getOwnerAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnerAddressFunction, string>(getOwnerAddressFunction, blockParameter);
        }

        public Task<string> GetOwnerAddressQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnerAddressFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetStakedAmountQueryAsync(GetStakedAmountFunction getStakedAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetStakedAmountFunction, BigInteger>(getStakedAmountFunction, blockParameter);
        }

        public Task<BigInteger> GetStakedAmountQueryAsync(string address_, BlockParameter blockParameter = null)
        {
            var getStakedAmountFunction = new GetStakedAmountFunction();
            getStakedAmountFunction.Address_ = address_;

            return ContractHandler.QueryAsync<GetStakedAmountFunction, BigInteger>(getStakedAmountFunction, blockParameter);
        }

        public Task<GetStakeholderInformationOutputDTO> GetStakeholderInformationQueryAsync(GetStakeholderInformationFunction getStakeholderInformationFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeholderInformationFunction, GetStakeholderInformationOutputDTO>(getStakeholderInformationFunction, blockParameter);
        }

        public Task<GetStakeholderInformationOutputDTO> GetStakeholderInformationQueryAsync(string address_, BlockParameter blockParameter = null)
        {
            var getStakeholderInformationFunction = new GetStakeholderInformationFunction();
            getStakeholderInformationFunction.Address_ = address_;

            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeholderInformationFunction, GetStakeholderInformationOutputDTO>(getStakeholderInformationFunction, blockParameter);
        }

        public Task<GetStakeholdersOutputDTO> GetStakeholdersQueryAsync(GetStakeholdersFunction getStakeholdersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeholdersFunction, GetStakeholdersOutputDTO>(getStakeholdersFunction, blockParameter);
        }

        public Task<GetStakeholdersOutputDTO> GetStakeholdersQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeholdersFunction, GetStakeholdersOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetValidatorsCountQueryAsync(GetValidatorsCountFunction getValidatorsCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetValidatorsCountFunction, BigInteger>(getValidatorsCountFunction, blockParameter);
        }

        public Task<BigInteger> GetValidatorsCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetValidatorsCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> IncreaseAllowanceRequestAsync(IncreaseAllowanceFunction increaseAllowanceFunction)
        {
            return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(IncreaseAllowanceFunction increaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(string spender, BigInteger addedValue)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
            increaseAllowanceFunction.Spender = spender;
            increaseAllowanceFunction.AddedValue = addedValue;

            return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger addedValue, CancellationTokenSource cancellationToken = null)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
            increaseAllowanceFunction.Spender = spender;
            increaseAllowanceFunction.AddedValue = addedValue;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<string> StakeRequestAsync(StakeFunction stakeFunction)
        {
            return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(StakeFunction stakeFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(BigInteger amount_)
        {
            var stakeFunction = new StakeFunction();
            stakeFunction.Amount_ = amount_;

            return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(BigInteger amount_, CancellationTokenSource cancellationToken = null)
        {
            var stakeFunction = new StakeFunction();
            stakeFunction.Amount_ = amount_;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StartNewEraRequestAsync(StartNewEraFunction startNewEraFunction)
        {
            return ContractHandler.SendRequestAsync(startNewEraFunction);
        }

        public Task<string> StartNewEraRequestAsync()
        {
            return ContractHandler.SendRequestAsync<StartNewEraFunction>();
        }

        public Task<TransactionReceipt> StartNewEraRequestAndWaitForReceiptAsync(StartNewEraFunction startNewEraFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(startNewEraFunction, cancellationToken);
        }

        public Task<TransactionReceipt> StartNewEraRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<StartNewEraFunction>(null, cancellationToken);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
            return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string to, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
            transferFunction.To = to;
            transferFunction.Amount = amount;

            return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
            transferFunction.To = to;
            transferFunction.Amount = amount;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
            return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string from, string to, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.Amount = amount;

            return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.Amount = amount;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(UnstakeFunction unstakeFunction)
        {
            return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(UnstakeFunction unstakeFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(BigInteger amount_)
        {
            var unstakeFunction = new UnstakeFunction();
            unstakeFunction.Amount_ = amount_;

            return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(BigInteger amount_, CancellationTokenSource cancellationToken = null)
        {
            var unstakeFunction = new UnstakeFunction();
            unstakeFunction.Amount_ = amount_;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }
    }
}