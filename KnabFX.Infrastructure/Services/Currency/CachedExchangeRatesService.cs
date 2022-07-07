
using KnabFX.Application.Models;
using KnabFX.Application.Services.Cache;
using KnabFX.Application.Services.Currency;
using KnabFX.Domain.Entities.Currency;

namespace KnabFX.Infrastructure.Services.Currency
{
    public class CachedExchangeRatesService : IExchangeRatesService
    {
        private readonly ICacheProvider<CurrencyRate> _cacheProvider;
        private readonly IExchangeRatesService _exchangeRatesService;

        public CachedExchangeRatesService(IExchangeRatesService exchangeRatesService, ICacheProvider<CurrencyRate> cacheProvider)
        {
            _cacheProvider = cacheProvider;
            _exchangeRatesService = exchangeRatesService;
        }

        public async Task<CurrencyRate> GetCurrencyRates(string baseCurrency = "")
        {
            CurrencyRate currencyRate = _cacheProvider.Get(baseCurrency);
            
            if(currencyRate == null)
            {
                currencyRate = await _exchangeRatesService.GetCurrencyRates(baseCurrency);

                _cacheProvider.Set(baseCurrency, currencyRate);
            }

            return currencyRate;
        }
    }
}
