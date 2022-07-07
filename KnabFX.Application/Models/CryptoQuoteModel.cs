using KnabFX.Domain.Entities.Cryptocurrency;
using Newtonsoft.Json;

namespace KnabFX.Application.Models
{
    public class CryptoQuoteModel
    {
        [JsonProperty("symbol")]
        public string Symbol;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("last_updated")]
        public DateTime LastUpdatedOn;

        [JsonProperty("quote")]
        public Dictionary<string, CryptoQuotePrice> Quote { get; set; } = new();

    }
}
