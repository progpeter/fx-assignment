using KnabFX.Application.Models;
using KnabFX.Domain.Entities.Currency;

namespace KnabFX.Application.Services.Currency
{
    public interface IExchangeRatesService
    {
        public Task<CurrencyRate> GetCurrencyRates(string baseCurrency = "");
    }
}
