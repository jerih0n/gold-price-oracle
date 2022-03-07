using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace GoldPriceOracle.Connection.Blockchain.Contracts.Timer
{
    public partial class TimerDeployment : TimerDeploymentBase
    {
        public TimerDeployment() : base(BYTECODE)
        {
        }

        public TimerDeployment(string byteCode) : base(byteCode)
        {
        }
    }

    public class TimerDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "610120604052610011603c6003610084565b608052610021603c610b40610084565b60a052600080805560015534801561003857600080fd5b50604051610752380380610752833981016040819052610057916100cd565b610100526001600160a01b0391821660c0521660e052600280546001600160a01b03191633179055610109565b60008160001904831182151516156100ac57634e487b7160e01b600052601160045260246000fd5b500290565b80516001600160a01b03811681146100c857600080fd5b919050565b6000806000606084860312156100e257600080fd5b6100eb846100b1565b92506100f9602085016100b1565b9150604084015190509250925092565b60805160a05160c05160e05161010051610602610150600039600050506000818161022701526103730152600060b6015260006103360152600061013e01526106026000f3fe608060405234801561001057600080fd5b50600436106100365760003560e01c80633e5073ac1461003b578063acac304f14610050575b600080fd5b61004e6100493660046104a4565b610063565b005b61004e61005e3660046104a4565b6101d9565b6002546001600160a01b031633146100b25760405162461bcd60e51b815260206004820152600d60248201526c1058d8d95cdcc811195b9a5959609a1b60448201526064015b60405180910390fd5b60007f00000000000000000000000000000000000000000000000000000000000000006001600160a01b0316636de7da786040518163ffffffff1660e01b8152600401602060405180830381865afa158015610112573d6000803e3d6000fd5b505050506040513d601f19601f8201168201806040525081019061013691906104bd565b9050816101637f0000000000000000000000000000000000000000000000000000000000000000836104ec565b1015801561017357508060005414155b156101ac576040518281527fb4992463ce91f2fd47fb1c5cf70d9866ebc0ea845740846fc0eb6b40ace688969060200160405180910390a15b6101b781600161050b565b60005414156101d5576000805490806101cf83610523565b91905055505b5050565b6002546001600160a01b031633146102235760405162461bcd60e51b815260206004820152600d60248201526c1058d8d95cdcc811195b9a5959609a1b60448201526064016100a9565b60007f00000000000000000000000000000000000000000000000000000000000000006001600160a01b031663533aa7b86040518163ffffffff1660e01b8152600401602060405180830381865afa158015610283573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906102a791906104bd565b905080610330576040516bffffffffffffffffffffffff193360601b166020820181905260348201849052605482015260009060680160408051601f19818403018152828252805160209182012086845290830181905292507ffe268d8e389bd2854e06f65a9e1d56d61f072f36cb34d8f8d722f6fe56521d26910160405180910390a1505050565b8161035b7f0000000000000000000000000000000000000000000000000000000000000000836104ec565b11801561036a57508060015414155b156104805760007f00000000000000000000000000000000000000000000000000000000000000006001600160a01b031663d250b6cb6040518163ffffffff1660e01b815260040160a060405180830381865afa1580156103cf573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906103f3919061053e565b8051604080830151608084015182516020810194909452918301526bffffffffffffffffffffffff19606091821b169082015290915060009060740160408051601f19818403018152828252805160209182012087845290830181905292507ffe268d8e389bd2854e06f65a9e1d56d61f072f36cb34d8f8d722f6fe56521d26910160405180910390a150505b61048b81600161050b565b60015414156101d557600180549060006101cf83610523565b6000602082840312156104b657600080fd5b5035919050565b6000602082840312156104cf57600080fd5b5051919050565b634e487b7160e01b600052601160045260246000fd5b6000816000190483118215151615610506576105066104d6565b500290565b6000821982111561051e5761051e6104d6565b500190565b6000600019821415610537576105376104d6565b5060010190565b600060a0828403121561055057600080fd5b60405160a0810181811067ffffffffffffffff8211171561058157634e487b7160e01b600052604160045260246000fd5b6040908152835182526020808501519083015283810151908201526060808401519082015260808301516001600160a01b03811681146105c057600080fd5b6080820152939250505056fea26469706673582212207dd2dbc1acd58b70fc1514207394cdbcc16ba5137079cd417f24789e379d95cb64736f6c634300080c0033";

        public TimerDeploymentBase() : base(BYTECODE)
        {
        }

        public TimerDeploymentBase(string byteCode) : base(byteCode)
        {
        }

        [Parameter("address", "resolverAddress_", 1)]
        public virtual string ResolverAddress_ { get; set; }

        [Parameter("address", "erasMonitor_", 2)]
        public virtual string ErasMonitor_ { get; set; }

        [Parameter("uint256", "initialTimeStamp_", 3)]
        public virtual BigInteger InitialTimeStamp_ { get; set; }
    }

    public partial class NewEraTimeElapsedFunction : NewEraTimeElapsedFunctionBase
    { }

    [Function("newEraTimeElapsed")]
    public class NewEraTimeElapsedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "utcTimeStamp_", 1)]
        public virtual BigInteger UtcTimeStamp_ { get; set; }
    }

    public partial class NewPriceRoundTimeElapsedFunction : NewPriceRoundTimeElapsedFunctionBase
    { }

    [Function("newPriceRoundTimeElapsed")]
    public class NewPriceRoundTimeElapsedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "utcTimeStamp_", 1)]
        public virtual BigInteger UtcTimeStamp_ { get; set; }
    }

    public partial class StarNewPriceRoundEventDTO : StarNewPriceRoundEventDTOBase
    { }

    [Event("StarNewPriceRound")]
    public class StarNewPriceRoundEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "utcTimeStamp", 1, false)]
        public virtual BigInteger UtcTimeStamp { get; set; }
    }

    public partial class StartNewEraEventDTO : StartNewEraEventDTOBase
    { }

    [Event("StartNewEra")]
    public class StartNewEraEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "utcTimeStamp_", 1, false)]
        public virtual BigInteger UtcTimeStamp_ { get; set; }

        [Parameter("bytes32", "newEraId_", 2, false)]
        public virtual byte[] NewEraId_ { get; set; }
    }
}