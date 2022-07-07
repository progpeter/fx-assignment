using KnabFX.Application;
using KnabFX.Application.Providers;
using KnabFX.Application.Services.Cache;
using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Application.Services.Currency;
using KnabFX.Infrastructure.Providers;
using KnabFX.Infrastructure.Services.Cache;
using KnabFX.Infrastructure.Services.Cryptocurrency;
using KnabFX.Infrastructure.Services.Currency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KnabFX.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.RegisterApplicationServices();

            services.AddScoped(typeof(ICacheProvider<>), typeof(MemoryCacheProvider<>));

            services.AddSingleton<CryptoRatesServiceConfig>((service) =>
            {
                var cryptoRatesServiceConfig = new CryptoRatesServiceConfig();
                configuration.Bind("ExternalServices:CryptoRatesServiceConfig", cryptoRatesServiceConfig);
                return cryptoRatesServiceConfig;
            });

            services.AddSingleton<ExchangeRatesServiceConfig>((service) =>
            {
                var exchangeRatesServiceConfig = new ExchangeRatesServiceConfig();
                configuration.Bind("ExternalServices:ExchangeRatesServiceConfig", exchangeRatesServiceConfig);
                return exchangeRatesServiceConfig;
            });

            services.AddHttpClient<IExchangeRatesService, ExchangeRatesService>();

            services.Decorate<IExchangeRatesService, CachedExchangeRatesService>();

            services.AddHttpClient<ICryptoRatesService, CryptoRatesService>();

            services.Decorate<ICryptoRatesService, CachedCryptoRatesService>();

            services.AddScoped<ICryptoRatesProvider, CryptoRatesProvider>();

            return services;
        }
    }
}
