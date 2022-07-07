using Microsoft.Extensions.Options;

namespace KnabFX.Infrastructure.Common
{
    public class ExternalServiceBase<TConfig> where TConfig : ExternalServiceBaseConfig
    {
        public HttpClient httpClient { get; }

        public TConfig clientConfig { get; set; }

        public ExternalServiceBase(HttpClient httpClient, TConfig clientConfig)
        {
            this.httpClient = httpClient;
            
            this.clientConfig = clientConfig;

            ConfigureClient();
        }

        private void ConfigureClient()
        {
            this.httpClient.BaseAddress = new Uri(clientConfig.BaseAddress);
            foreach (KeyValuePair<string, string> header in clientConfig.Headers.ToList())
            {
                this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
