using AutoMapper;
using KnabFX.Application.Mapping;
using KnabFX.Domain.Entities.Currency;
using KnabFX.Infrastructure.Services.Currency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KnabFX.Infrastructure.Tests
{
    public class ExchangeRatesServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExchangeRatesService> _logger = Substitute.For<ILogger<ExchangeRatesService>>();
        private readonly IMapper _mapper;

        public ExchangeRatesServiceTests()
        {
            _configuration = SetupInMemoryConfig();
            _mapper = SetupAutoMapper();
        }

        private IMapper SetupAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            return config.CreateMapper();
        }

        private IConfiguration SetupInMemoryConfig()
        {
            var inMemorySettings = new Dictionary<string, string> {
                                    {"Currency:BaseCurrency", "USD"},
                                    {"Currency:ConvertableCurrencies","EUR,GBP,AUD,BRL" }
            };

            return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        }

        [Theory]
        [InlineData("USD")]
        public async Task GetCurrencyRates_ShouldReturnNull_WhenCurrencyIsValid(string baseCurrency)
        {
            // Arrange
            string successApiResponse = "{\"base\":\"USD\",\"date\":\"2022-07-02\",\"rates\":{\"AUD\":1.467352,\"BRL\":5.33245,\"EUR\":0.958904,\"GBP\":0.826105},\"success\":true,\"timestamp\":1656727565}";

            var messageHandler = new MockHttpMessageHandler(successApiResponse);

            var _httpClient = new HttpClient(messageHandler);

            var exchangeRatesServiceConfig = new ExchangeRatesServiceConfig()
            {
                BaseAddress = "https://api.apilayer.com/",
                DefaultApi = "/exchangerates_data/latest"
            };

            ExchangeRatesService _sut = new ExchangeRatesService(_httpClient, exchangeRatesServiceConfig, _logger, _configuration, _mapper);


            // Act
            CurrencyRate currencyRate = await _sut.GetCurrencyRates(baseCurrency);


            // Assert
            currencyRate.Should().NotBeNull();
            currencyRate.BaseCurrency.Should().Be(baseCurrency);
            currencyRate.Rates.Should().ContainKeys("AUD");
            currencyRate.Rates.Should().ContainKeys("GBP");
            currencyRate.Rates.Should().ContainKeys("BRL");
            currencyRate.Rates.Should().ContainKeys("EUR");
            currencyRate.Rates["EUR"].Should().Be(0.958904);
        }

        [Theory]
        [InlineData("XYZ")]
        public async Task GetCurrencyRates_ShouldReturnRates_WhenCurrencyDoesNotExist(string baseCurrency)
        {
            // Arrange
            string failureApiResponse = "{\"success\":false,\"error\":{\"code\":201,\"type\":\"invalid_base_currency\"}}";

            var messageHandler = new MockHttpMessageHandler(failureApiResponse);

            var _httpClient = new HttpClient(messageHandler);

            var exchangeRatesServiceConfig = new ExchangeRatesServiceConfig()
            {
                BaseAddress = "https://api.apilayer.com/",
                DefaultApi = "/fixer/latest"
            };

            ExchangeRatesService _sut = new ExchangeRatesService(_httpClient, exchangeRatesServiceConfig, _logger, _configuration, _mapper);


            // Act
            CurrencyRate currencyRate= await _sut.GetCurrencyRates(baseCurrency);


            // Assert
            currencyRate.Should().BeNull();
        }

       
    }
}
