using KnabFX.Application.Services.Cache;
using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Domain.Entities.Cryptocurrency;

namespace KnabFX.Infrastructure.Services.Cryptocurrency
{
    public class CachedCryptoRatesService : ICryptoRatesService
    {
        private readonly ICryptoRatesService _cryptoRatesService;
        private readonly ICacheProvider<CryptoQuote> _cacheProvider;

        public CachedCryptoRatesService(ICryptoRatesService cryptoRatesService, ICacheProvider<CryptoQuote> cacheProvider)
        {
            _cryptoRatesService = cryptoRatesService;
            _cacheProvider = cacheProvider;
        }

        public async Task<CryptoQuote> GetCryptoQuote(string cryptoSymbol)
        {
            CryptoQuote cryptoQuote = _cacheProvider.Get(cryptoSymbol);

            if(cryptoQuote == null)
            {
                cryptoQuote = await _cryptoRatesService.GetCryptoQuote(cryptoSymbol);
                _cacheProvider.Set(cryptoSymbol, cryptoQuote);
            }

            return cryptoQuote;
        }
    }
}
