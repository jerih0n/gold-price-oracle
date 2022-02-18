﻿using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Infrastructure.Cryptography.AES;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.Information;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class InformationService : BaseAuthorizedService, IInformationService
    {
        private readonly IGoldPriceOracleERC20TokenService _goldPriceOracleERC20TokenService;
        public InformationService(INodeDataDataAccessService nodeDataDataAccessService, 
            IGoldPriceOracleERC20TokenService goldPriceOracleERC20TokenService) : 
            base(nodeDataDataAccessService)
        {
            _goldPriceOracleERC20TokenService = goldPriceOracleERC20TokenService;
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

        public async Task<TryResult<OracleTokenBalance>> GetTokenBalanceAsync(string password)
        {
            try
            {
                var autorizeResult = Authorize(password);
                if (!autorizeResult.Item1)
                {
                    return TryResult<OracleTokenBalance>.Fail(autorizeResult.Item2);
                }

                var nodeData = autorizeResult.Item3;

                var result = await _goldPriceOracleERC20TokenService.GetBalance(nodeData.ActiveAddress);
                var balanceAsString = result.ToString();

                return  TryResult<OracleTokenBalance>.Success(new OracleTokenBalance(balanceAsString, null));
            }
            catch(Exception ex)
            {
                return TryResult<OracleTokenBalance>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }

            throw new NotImplementedException();
        }
    }
}
