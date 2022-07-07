using KnabFX.Application.Providers;
using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Application.Services.Currency;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.Domain.Entities.Currency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KnabFX.Infrastructure.Providers
{
    public class CryptoRatesProvider : ICryptoRatesProvider
    {
        private readonly ICryptoRatesService _cryptoRatesService;
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly ILogger<CryptoRatesProvider> _logger;
        private readonly IConfiguration _configuration;

        public CryptoRatesProvider(ICryptoRatesService cryptoRatesService, 
            IExchangeRatesService exchangeRatesService, ILogger<CryptoRatesProvider> logger,
            IConfiguration configuration)
        {
            _cryptoRatesService = cryptoRatesService;
            _exchangeRatesService = exchangeRatesService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<CryptoRate>> GetCryptoRates(string cryptoSymbol)
        {
            if (string.IsNullOrEmpty(cryptoSymbol) || cryptoSymbol.Length > 5)
            {
                _logger.LogError("Invalid Crypto Symbol");
                return null;
            }

            string _baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

            _logger.LogInformation($"Getting Crypto Quote for {cryptoSymbol}");
            
            CryptoQuote cryptoQuote = await _cryptoRatesService.GetCryptoQuote(cryptoSymbol.ToUpper());

            if (cryptoQuote == null)
            {
                _logger.LogError($"Failed to get Crypto Quote for {cryptoSymbol}");
                return null;
            }
            
            List<CryptoRate> cryptoRates = new List<CryptoRate>();

            _logger.LogInformation($"Getting Base Currency Exchange Rates for {_baseCurrency}");

            CurrencyRate currencyRate = await _exchangeRatesService.GetCurrencyRates(_baseCurrency);

            if (!cryptoQuote.Quote.ContainsKey(_baseCurrency))
            {
                _logger.LogError($"Failed to get Base Currency Exchange Rates for {cryptoSymbol}");
                return null;
            }

            double baseCurrencyPrice = cryptoQuote.Quote[_baseCurrency].Price;
            
            // Add base crypto / base_currency rate
            cryptoRates.Add(new CryptoRate(cryptoQuote.Symbol, _baseCurrency, baseCurrencyPrice, DateTimeOffset.FromUnixTimeSeconds(cryptoQuote.LastUpdatedOnSeconds)));
           
            if (currencyRate == null)
            {
                return cryptoRates;
            }

            DateTimeOffset lastUpdatedOn = DateTimeOffset.FromUnixTimeSeconds(Math.Max(cryptoQuote.LastUpdatedOnSeconds, currencyRate.LastUpdatedOn));

            // Convert crypto/base_currency into other convertiable currencies
            foreach (var currency in currencyRate.Rates)
            {
                double convertedCurrency = baseCurrencyPrice * currency.Value;

                cryptoRates.Add(new CryptoRate(cryptoQuote.Symbol, currency.Key, convertedCurrency, lastUpdatedOn));
            }

            return cryptoRates;
        }
    }
}
