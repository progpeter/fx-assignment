using AutoMapper;
using KnabFX.Application.Mapping;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.Infrastructure.Services.Cryptocurrency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KnabFX.Infrastructure.Tests
{
    public class CryptoRatesServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CryptoRatesService> _logger = Substitute.For<ILogger<CryptoRatesService>>();
        private readonly IMapper _mapper;

        public CryptoRatesServiceTests()
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
                                    {"Currency:BaseCurrency", "USD"} };

            return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        }

        [Theory]
        [InlineData("BTC")]
        public async Task GetCryptoQuote_ShouldReturnQuote_WhenCryptoSymbolIsValid(string cryptoSymbol)
        {
            // Arrange
            string baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

            string successApiResponse = "{\"status\":{\"timestamp\":\"2022-07-07T02:07:46.214Z\",\"error_code\":0,\"error_message\":null,\"elapsed\":0,\"credit_count\":1,\"notice\":null},\"data\":{\"BTC\":{\"symbol\":\"BTC\",\"id\":\"rfwcwf3o5xc\",\"name\":\"3keti3dzraw\",\"amount\":1,\"last_updated\":\"2022-07-07T02:07:46.214Z\",\"quote\":{\"USD\":{\"price\":2980,\"last_updated\":\"2022-07-07T02:07:46.214Z\"}}}}}";

            var messageHandler = new MockHttpMessageHandler(successApiResponse);

            var _httpClient = new HttpClient(messageHandler);

            var cryptoRatesServiceConfig = new CryptoRatesServiceConfig()
            {
                BaseAddress = "https://sandbox-api.coinmarketcap.com/",
                DefaultApi = "/v2/tools/price-conversion"
            };

            CryptoRatesService _sut = new CryptoRatesService(_httpClient, cryptoRatesServiceConfig, _logger, _configuration, _mapper);


            // Act
            CryptoQuote cryptoQuote = await _sut.GetCryptoQuote(cryptoSymbol);


            // Assert
            cryptoQuote.Should().NotBeNull();
            cryptoQuote.Symbol.Should().Be(cryptoSymbol);
            cryptoQuote.Quote.Should().ContainKeys(baseCurrency);
            cryptoQuote.Quote[baseCurrency].Price.Should().Be(2980);
        }

        [Theory]
        [InlineData("ABCDF")]
        public async Task GetCryptoQuote_ShouldReturnNull_WhenCryptoSymbolDoesNotExist(string cryptoSymbol)
        {
            // Arrange
            string baseCurrency = _configuration.GetValue<string>("Currency:BaseCurrency").ToUpper();

            string failureApiResponse = "{\"status\":{\"timestamp\":\"2022-07-07T02:07:46.214Z\",\"error_code\":100,\"error_message\":null,\"elapsed\":0,\"credit_count\":1,\"notice\":null},\"data\":{\"BTC\":{\"symbol\":\"BTC\",\"id\":\"rfwcwf3o5xc\",\"name\":\"3keti3dzraw\",\"amount\":1,\"last_updated\":\"2022-07-07T02:07:46.214Z\",\"quote\":{\"USD\":{\"price\":2980,\"last_updated\":\"2022-07-07T02:07:46.214Z\"}}}}}";
            
            var messageHandler = new MockHttpMessageHandler(failureApiResponse);

            var _httpClient = new HttpClient(messageHandler);

            var cryptoRatesServiceConfig = new CryptoRatesServiceConfig()
            {
                BaseAddress = "https://sandbox-api.coinmarketcap.com/",
                DefaultApi = "/v2/tools/price-conversion"
            };

            CryptoRatesService _sut = new CryptoRatesService(_httpClient, cryptoRatesServiceConfig, _logger, _configuration, _mapper);


            // Act
            CryptoQuote cryptoQuote = await _sut.GetCryptoQuote(cryptoSymbol);


            // Assert
            cryptoQuote.Should().BeNull();
        }

    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;

        public MockHttpMessageHandler(string response)
        {
            _response = response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(_response)
            });
        }
    }
}
