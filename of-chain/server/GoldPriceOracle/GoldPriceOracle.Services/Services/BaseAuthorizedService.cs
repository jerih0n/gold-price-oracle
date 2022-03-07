using GoldPriceOracle.Connection.Database;
using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Cryptography.Password;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using System.Net;

namespace GoldPriceOracle.Services.Services
{
    public class BaseAuthorizedService
    {
        public BaseAuthorizedService(INodeDataDataAccessService nodeDataDataAccessService)
        {
            NodeDataAccessService = nodeDataDataAccessService;
        }

        protected (bool, ApiError, NodeData) Authorize(string password)
        {
            var nodeData = NodeDataAccessService.GetNodeData();

            if (nodeData == null)
            {
                return (false, new ApiError(HttpStatusCode.NotFound, "No node data found. Set node first"), null);
            }

            var isPasswordMatching = PasswordCryptoProvider.IsValidPassword(password, nodeData.Password);

            if (!isPasswordMatching)
            {
                return (false, new ApiError(HttpStatusCode.Unauthorized, "Wrong password!"), null);
            }

            return (true, null, nodeData);
        }

        protected INodeDataDataAccessService NodeDataAccessService { get; }
    }
}