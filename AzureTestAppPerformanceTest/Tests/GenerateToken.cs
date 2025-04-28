using AzureTestAppPerformanceTest.config;
using AzureTestAppPerformanceTest.Common;
using AzureTestAppPerformanceTest.Exceptions;
using VRI.Integrations.Connectors.Interfaces;
using VRI.Integrations.Connectors.Models;

namespace AzureTestAppPerformanceTest
{
    public class GenerateToken
    {
        private readonly IGenericTokenClient _genericTokenClient;
        private readonly IShopifyConfiguration _shopifyConfiguration;
        public GenerateToken(
            IGenericTokenClient genericTokenClient,
            IShopifyConfiguration shopifyConfiguration)
        {
            _genericTokenClient = genericTokenClient;
            _shopifyConfiguration = shopifyConfiguration;
        }

        public async Task<string?> GetTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_shopifyConfiguration.GrantType) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.ClientId) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.ClientSecret) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.Scope))
            {
                throw new GenericNullException(Constants.ErrorMessages.ErrorWithNull);
            }

            TokenRequestModel tokenRequest = GenerateTokenRequest();

            var token = await _genericTokenClient.GetTokenAsync(tokenRequest);

            if (string.IsNullOrEmpty(token))
                throw new GenericException(Constants.ErrorMessages.InvalidOrNullToken);

            return token;
        }

        private TokenRequestModel GenerateTokenRequest()
        {
            return new TokenRequestModel
            {
                GrantType = _shopifyConfiguration.GrantType,
                ClientId = _shopifyConfiguration.ClientId,
                ClientSecret = _shopifyConfiguration.ClientSecret,
                Scope = _shopifyConfiguration.Scope,
                RetryCount = _shopifyConfiguration.RetryCount,
                TokenUrl = _shopifyConfiguration.TokenUrl,
                ExistingToken = string.Empty
            };
        }
    }
}
