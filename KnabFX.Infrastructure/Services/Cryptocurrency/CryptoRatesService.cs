using AutoMapper;
using KnabFX.Application.Models;
using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnabFX.Infrastructure.Services.Cryptocurrency
{
    public class CryptoRatesService : ExternalServiceBase<CryptoRatesServiceConfig>, ICryptoRatesService
    {
        private readonly ILogger<CryptoRatesService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CryptoRatesService(HttpClient httpClient, CryptoRatesServiceConfig clientConfig,
            ILogger<CryptoRatesService> logger, IConfiguration configuration, IMapper mapper) : base(httpClient, clientConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CryptoQuote> GetCryptoQuote(string cryptoSymbol)
        {
            try
            {
                if (string.IsNullOrEmpty(cryptoSymbol) || cryptoSymbol.Length > 6) return null;

                cryptoSymbol = cryptoSymbol.ToUpper();

                string _baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

                string response = await httpClient.GetStringAsync($"{clientConfig.DefaultApi}?symbol={cryptoSymbol}&convert={_baseCurrency}&amount=1");

                if (!string.IsNullOrEmpty(response))
                {
                    // Since the API response are different between Sandbox and Live APIs we are handling each one separately
                    CryptoQuoteModel cryptoQuoteModel = GetQuoteFromResponse(response, cryptoSymbol);

                    if (cryptoQuoteModel != null)
                        return _mapper.Map<CryptoQuote>(cryptoQuoteModel);
                }


                _logger.LogError($"{nameof(CryptoRatesService)} :: Failed to get crypto quote for {cryptoSymbol}", response);

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"{nameof(CryptoRatesService)} :: Failed to get crypto quote for {cryptoSymbol}", ex.Message);
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError($"{nameof(CryptoRatesService)} :: Failed to parse Crypto Quotes API response", ex.Message);
            }

            return null;
        }

        private CryptoQuoteModel GetQuoteFromResponse(string response, string cryptoSymbol)
        {
            var apiResponse = JsonConvert.DeserializeObject<CryptoRateApiResponse>(response);

            if (apiResponse != null && apiResponse.Status.ErrorCode == 0 && apiResponse.Data != null)
            {
                if (apiResponse.Data.Type == JTokenType.Object)
                {
                    return apiResponse.Data[cryptoSymbol].ToObject<CryptoQuoteModel>();
                }
                else
                    return apiResponse.Data[0].ToObject<CryptoQuoteModel>();
            }

            return null;
        }
    }
}
