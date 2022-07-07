using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Application.Services.Currency;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.Domain.Entities.Currency;
using KnabFX.Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KnabFX.Infrastructure.Tests
{
    public class CryptoRatesProviderTests
    {
        private readonly CryptoRatesProvider _sut;
        private readonly ICryptoRatesService _cryptoRatesService = Substitute.For<ICryptoRatesService>();
        private readonly IExchangeRatesService _exchangeRatesService = Substitute.For<IExchangeRatesService>();
        private readonly ILogger<CryptoRatesProvider> _logger = Substitute.For<ILogger<CryptoRatesProvider>>();
        private readonly IConfiguration _configuration;


        public CryptoRatesProviderTests()
        {
            _configuration = SetupInMemoryConfig();
            _sut = new CryptoRatesProvider(_cryptoRatesService, _exchangeRatesService, _logger, _configuration);
        }

        private IConfiguration SetupInMemoryConfig()
        {
            var inMemorySettings = new Dictionary<string, string> {
                                    {"Currency:BaseCurrency", "USD"} };

            return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        [InlineData("SOL")]
        public async Task GetCryptoRates_ShouldReturnRates_WhenCryptoSymbolIsValid(string cryptoSymbol)
        {
            // Arrange
            string baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

            var cryptoQuote = new CryptoQuote()
            {
                Symbol = cryptoSymbol,
                LastUpdatedOn = DateTime.UtcNow,
                Quote = new Dictionary<string, CryptoQuotePrice>()
                {
                    {baseCurrency, new CryptoQuotePrice(10, DateTime.Now) }
                }
            };

            _cryptoRatesService.GetCryptoQuote(cryptoSymbol).Returns(cryptoQuote);


            var baseCurrencyRates = new CurrencyRate()
            {
                BaseCurrency = baseCurrency,
                LastUpdatedOn = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Rates = new Dictionary<string, double>()
                {
                    {"EUR", 2 },
                    { "GBP", 0.5}
                }
            };

            _exchangeRatesService.GetCurrencyRates(baseCurrency).Returns(baseCurrencyRates);


            // Act
            var cryptoRates = await _sut.GetCryptoRates(cryptoSymbol);


            // Assert
            cryptoRates.Should().HaveCount(3);
            cryptoRates.Should().Satisfy(
                c => c.ExCurrency == "USD" && c.Price == 10,
                c => c.ExCurrency == "EUR" && c.Price == 20,
                c => c.ExCurrency == "GBP" && c.Price == 5);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("ABCDEF")]
        public async Task GetCryptoRates_ShouldReturnNull_WhenCryptoSymbolIsNotValidOrEmpty(string cryptoSymbol)
        {
            // Arrange
            string baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

            _cryptoRatesService.GetCryptoQuote(cryptoSymbol).Returns(Task.FromResult<CryptoQuote>(null));


            var baseCurrencyRates = new CurrencyRate()
            {
                BaseCurrency = baseCurrency,
                LastUpdatedOn = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Rates = new Dictionary<string, double>()
                {
                    {"EUR", 2 },
                    { "GBP", 0.5}
                }
            };

            _exchangeRatesService.GetCurrencyRates(baseCurrency).Returns(baseCurrencyRates);


            // Act
            var cryptoRates = await _sut.GetCryptoRates(cryptoSymbol);

            // Assert
            cryptoRates.Should().BeNull();
        }
    }
}
