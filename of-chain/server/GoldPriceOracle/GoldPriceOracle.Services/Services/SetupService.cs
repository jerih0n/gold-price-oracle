using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Accounts;
using GoldPriceOracle.Infrastructure.Cryptography.AES;
using GoldPriceOracle.Infrastructure.Cryptography.Password;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.Setup;
using Nethereum.Web3.Accounts;
using System;
using System.Net;

namespace GoldPriceOracle.Services.Services
{
    public class SetupService : ISetupService
    {
        private readonly INodeDataDataAccessService _nodeDataAccessService;
        private readonly IHDWalletManagingService _HDWalletManagingService;

        public SetupService(INodeDataDataAccessService nodeDataDataAccessService, IHDWalletManagingService hDWalletManagingService)
        {
            _nodeDataAccessService = nodeDataDataAccessService;
            _HDWalletManagingService = hDWalletManagingService;
        }

        public TryResult<IsNodeSetUpModel> IsNodeSetup()
        {
            try
            {
                var isAlreadySet = IsAlreadySet();
                return TryResult<IsNodeSetUpModel>.Success(new IsNodeSetUpModel(isAlreadySet));
            }
            catch(Exception ex)
            {
                return TryResult<IsNodeSetUpModel>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public TryResult<IsNodeSetUpModel> RestoreFromMnemonic(string mnemonic, string password)
        {
            try
            {
                if(IsAlreadySet())
                {
                    return TryResult<IsNodeSetUpModel>.Fail(new ApiError(HttpStatusCode.BadRequest, "Node is already set"));
                }

                var account = _HDWalletManagingService.RestoreWalletFromMnemonic(mnemonic);
                var address = account.Address;
                var privateKey = account.PrivateKey;

                var encryptedPassoword = PasswordCryptoProvider.EncryptPassword(password);
                var encryptedPrivateKey = AESCryptoProvider.Encrypt(privateKey, password);
                var encryptedMnemonic = AESCryptoProvider.Encrypt(mnemonic, password);

                var isSet = _nodeDataAccessService.CreateNewNode(encryptedPassoword, encryptedPrivateKey, address, encryptedMnemonic);

                return TryResult<IsNodeSetUpModel>.Success(new IsNodeSetUpModel(isSet));
            }
            catch(Exception ex)
            {
                return TryResult<IsNodeSetUpModel>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public TryResult<IsNodeSetUpModel> SetupNode(string password)
        {
            try
            {
                if (IsAlreadySet())
                {
                    return TryResult<IsNodeSetUpModel>.Fail(new ApiError(HttpStatusCode.BadRequest, "Node is already set"));
                }

                (Account account, string mnemonic) = _HDWalletManagingService.CreateNewWallet();
                var address = account.Address;
                var privateKey = account.PrivateKey;

                var encryptedPassoword = PasswordCryptoProvider.EncryptPassword(password);
                var encryptedPrivateKey = AESCryptoProvider.Encrypt(privateKey, password);
                var encryptedMnemonic = AESCryptoProvider.Encrypt(mnemonic, password);

                var isSet = _nodeDataAccessService.CreateNewNode(encryptedPassoword, encryptedPrivateKey, address, encryptedMnemonic);

                return TryResult<IsNodeSetUpModel>.Success(new IsNodeSetUpModel(isSet));
            }
            catch(Exception ex)
            {
                return TryResult<IsNodeSetUpModel>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
            
        }

        private bool IsAlreadySet()
        {
            var nodeData =_nodeDataAccessService.GetNodeData();
            return nodeData != null;
        }
    }
}
