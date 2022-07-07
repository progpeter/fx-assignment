using KnabFX.Domain.Entities.Cryptocurrency;

namespace KnabFX.Application.Providers
{
    public interface ICryptoRatesProvider
    {
        Task<List<CryptoRate>> GetCryptoRates(string cryptoSymbol);
    }
}