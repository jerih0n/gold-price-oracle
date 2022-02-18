using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Cryptography.AES;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.Information;
using System;
using System.Net;

namespace GoldPriceOracle.Services.Services
{
    public class InformationService : BaseAuthorizedService, IInformationService
    {
        public InformationService(INodeDataDataAccessService nodeDataDataAccessService) : base(nodeDataDataAccessService)
        {
            
        }

        public TryResult<MnemonicPhraseModel> GetMnemonicPhrase(string password)
        {
            try
            {
                var autorizeResult = Authorize(password);
                if(!autorizeResult.Item1)
                {
                    return TryResult<MnemonicPhraseModel>.Fail(autorizeResult.Item2);
                }

                var nodeData = autorizeResult.Item3;

                var decryptedMnemonic = AESCryptoProvider.Decrypt(nodeData.MnemonicPhraseEncrypted, password);

                return TryResult<MnemonicPhraseModel>.Success(new MnemonicPhraseModel(decryptedMnemonic));
            }
            catch(Exception ex)
            {
                return TryResult<MnemonicPhraseModel>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public TryResult<AddressInformation> GetNodeActiveAddress(string password)
        {
            try
            {
                var autorizeResult = Authorize(password);
                if (!autorizeResult.Item1)
                {
                    return TryResult<AddressInformation>.Fail(autorizeResult.Item2);
                }

                var nodeData = autorizeResult.Item3;

                return TryResult<AddressInformation>.Success(new AddressInformation(nodeData.ActiveAddress));
            }
            catch(Exception ex)
            {
                return TryResult<AddressInformation>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
    }
}
