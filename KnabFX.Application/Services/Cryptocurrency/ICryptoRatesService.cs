using KnabFX.Domain.Entities.Cryptocurrency;

namespace KnabFX.Application.Services.Cryptocurrency
{
    public interface ICryptoRatesService
    {
        public Task<CryptoQuote> GetCryptoQuote(string cryptoSymbol);
    }
}
