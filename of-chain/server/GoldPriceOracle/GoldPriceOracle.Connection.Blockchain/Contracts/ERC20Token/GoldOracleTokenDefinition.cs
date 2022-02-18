using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace GoldPriceOracle.Connection.Blockchain.ERC20Token
{
    public partial class GoldOracleTokenDeployment : GoldOracleTokenDeploymentBase
    {
        public GoldOracleTokenDeployment() : base(BYTECODE) { }
        public GoldOracleTokenDeployment(string byteCode) : base(byteCode) { }
    }

    public class GoldOracleTokenDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b506040518060400160405280600f81526020016e23b7b63227b930b1b632aa37b5b2b760891b8152506040518060400160405280600381526020016211d3d560ea1b81525081600390805190602001906200006e929190620001a4565b50805162000084906004906020840190620001a4565b505050620000a4336aa56fa5b99019a5c8000000620000bc60201b60201c565b600580546001600160a01b03191633179055620002ae565b6001600160a01b038216620001175760405162461bcd60e51b815260206004820152601f60248201527f45524332303a206d696e7420746f20746865207a65726f206164647265737300604482015260640160405180910390fd5b80600260008282546200012b91906200024a565b90915550506001600160a01b038216600090815260208190526040812080548392906200015a9084906200024a565b90915550506040518181526001600160a01b038316906000907fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef9060200160405180910390a35050565b828054620001b29062000271565b90600052602060002090601f016020900481019282620001d6576000855562000221565b82601f10620001f157805160ff191683800117855562000221565b8280016001018555821562000221579182015b828111156200022157825182559160200191906001019062000204565b506200022f92915062000233565b5090565b5b808211156200022f576000815560010162000234565b600082198211156200026c57634e487b7160e01b600052601160045260246000fd5b500190565b600181811c908216806200028657607f821691505b60208210811415620002a857634e487b7160e01b600052602260045260246000fd5b50919050565b6108f480620002be6000396000f3fe608060405234801561001057600080fd5b50600436106100b45760003560e01c80633950935111610071578063395093511461014957806370a082311461015c57806395d89b4114610185578063a457c2d71461018d578063a9059cbb146101a0578063dd62ed3e146101b357600080fd5b806306fdde03146100b9578063095ea7b3146100d75780630c4f65bd146100fa57806318160ddd1461011557806323b872dd14610127578063313ce5671461013a575b600080fd5b6100c16101ec565b6040516100ce9190610731565b60405180910390f35b6100ea6100e53660046107a2565b61027e565b60405190151581526020016100ce565b6005546040516001600160a01b0390911681526020016100ce565b6002545b6040519081526020016100ce565b6100ea6101353660046107cc565b610296565b604051601281526020016100ce565b6100ea6101573660046107a2565b6102ba565b61011961016a366004610808565b6001600160a01b031660009081526020819052604090205490565b6100c16102f9565b6100ea61019b3660046107a2565b610308565b6100ea6101ae3660046107a2565b61039f565b6101196101c136600461082a565b6001600160a01b03918216600090815260016020908152604080832093909416825291909152205490565b6060600380546101fb9061085d565b80601f01602080910402602001604051908101604052809291908181526020018280546102279061085d565b80156102745780601f1061024957610100808354040283529160200191610274565b820191906000526020600020905b81548152906001019060200180831161025757829003601f168201915b5050505050905090565b60003361028c8185856103ad565b5060019392505050565b6000336102a48582856104d1565b6102af858585610563565b506001949350505050565b3360008181526001602090815260408083206001600160a01b038716845290915281205490919061028c90829086906102f4908790610898565b6103ad565b6060600480546101fb9061085d565b3360008181526001602090815260408083206001600160a01b0387168452909152812054909190838110156103925760405162461bcd60e51b815260206004820152602560248201527f45524332303a2064656372656173656420616c6c6f77616e63652062656c6f77604482015264207a65726f60d81b60648201526084015b60405180910390fd5b6102af82868684036103ad565b60003361028c818585610563565b6001600160a01b03831661040f5760405162461bcd60e51b8152602060048201526024808201527f45524332303a20617070726f76652066726f6d20746865207a65726f206164646044820152637265737360e01b6064820152608401610389565b6001600160a01b0382166104705760405162461bcd60e51b815260206004820152602260248201527f45524332303a20617070726f766520746f20746865207a65726f206164647265604482015261737360f01b6064820152608401610389565b6001600160a01b0383811660008181526001602090815260408083209487168084529482529182902085905590518481527f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925910160405180910390a3505050565b6001600160a01b03838116600090815260016020908152604080832093861683529290522054600019811461055d57818110156105505760405162461bcd60e51b815260206004820152601d60248201527f45524332303a20696e73756666696369656e7420616c6c6f77616e63650000006044820152606401610389565b61055d84848484036103ad565b50505050565b6001600160a01b0383166105c75760405162461bcd60e51b815260206004820152602560248201527f45524332303a207472616e736665722066726f6d20746865207a65726f206164604482015264647265737360d81b6064820152608401610389565b6001600160a01b0382166106295760405162461bcd60e51b815260206004820152602360248201527f45524332303a207472616e7366657220746f20746865207a65726f206164647260448201526265737360e81b6064820152608401610389565b6001600160a01b038316600090815260208190526040902054818110156106a15760405162461bcd60e51b815260206004820152602660248201527f45524332303a207472616e7366657220616d6f756e7420657863656564732062604482015265616c616e636560d01b6064820152608401610389565b6001600160a01b038085166000908152602081905260408082208585039055918516815290812080548492906106d8908490610898565b92505081905550826001600160a01b0316846001600160a01b03167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef8460405161072491815260200190565b60405180910390a361055d565b600060208083528351808285015260005b8181101561075e57858101830151858201604001528201610742565b81811115610770576000604083870101525b50601f01601f1916929092016040019392505050565b80356001600160a01b038116811461079d57600080fd5b919050565b600080604083850312156107b557600080fd5b6107be83610786565b946020939093013593505050565b6000806000606084860312156107e157600080fd5b6107ea84610786565b92506107f860208501610786565b9150604084013590509250925092565b60006020828403121561081a57600080fd5b61082382610786565b9392505050565b6000806040838503121561083d57600080fd5b61084683610786565b915061085460208401610786565b90509250929050565b600181811c9082168061087157607f821691505b6020821081141561089257634e487b7160e01b600052602260045260246000fd5b50919050565b600082198211156108b957634e487b7160e01b600052601160045260246000fd5b50019056fea2646970667358221220bdd6fa17ec4337ae4745d82acf3f8bd6cbafca599daf886ce4c6edce03d7198064736f6c634300080c0033";
        public GoldOracleTokenDeploymentBase() : base(BYTECODE) { }
        public GoldOracleTokenDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class DecreaseAllowanceFunction : DecreaseAllowanceFunctionBase { }

    [Function("decreaseAllowance", "bool")]
    public class DecreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "subtractedValue", 2)]
        public virtual BigInteger SubtractedValue { get; set; }
    }

    public partial class GetOwnerAddressFunction : GetOwnerAddressFunctionBase { }

    [Function("getOwnerAddress", "address")]
    public class GetOwnerAddressFunctionBase : FunctionMessage
    {

    }

    public partial class IncreaseAllowanceFunction : IncreaseAllowanceFunctionBase { }

    [Function("increaseAllowance", "bool")]
    public class IncreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "addedValue", 2)]
        public virtual BigInteger AddedValue { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }



    public partial class GetOwnerAddressOutputDTO : GetOwnerAddressOutputDTOBase { }

    [FunctionOutput]
    public class GetOwnerAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
