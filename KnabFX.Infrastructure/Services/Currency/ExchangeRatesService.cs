using AutoMapper;
using KnabFX.Application.Models;
using KnabFX.Application.Services.Currency;
using KnabFX.Domain.Entities.Currency;
using KnabFX.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KnabFX.Infrastructure.Services.Currency
{
    public class ExchangeRatesService : ExternalServiceBase<ExchangeRatesServiceConfig>, IExchangeRatesService
    {
        private readonly ILogger<ExchangeRatesService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ExchangeRatesService(HttpClient httpClient, ExchangeRatesServiceConfig clientConfig,
            ILogger<ExchangeRatesService> logger, IConfiguration configuration, IMapper mapper): base(httpClient, clientConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CurrencyRate> GetCurrencyRates(string baseCurrency = "")
        {
            if (string.IsNullOrEmpty(baseCurrency))
                baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();
                

            string convertableCurrencies = _configuration.GetValue<string>("Currency:ConvertableCurrencies");

            try {

                string response = await httpClient.GetStringAsync($"{clientConfig.DefaultApi}?symbols={convertableCurrencies}&base={baseCurrency}");

                var currencyRate =  JsonConvert.DeserializeObject<CurrencyRateModel>(response);

                if (currencyRate != null && currencyRate.Success)
                    return _mapper.Map<CurrencyRate>(currencyRate);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"{nameof(ExchangeRatesService)} :: Failed to get exchange rates for {baseCurrency}", ex.Message);
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError($"{nameof(ExchangeRatesService)} :: Failed to parse exchange rates API response", ex.Message);
            }

            return null;
        }
    }
}
