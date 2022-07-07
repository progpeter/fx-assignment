using Newtonsoft.Json;

namespace KnabFX.Application.Models
{
    public class CurrencyRateModel
    {
        public bool Success { get; set; }

        [JsonProperty("base")]
        public string BaseCurrency { get; set; }

        public Dictionary<string, double> Rates { get; set; } = new Dictionary<string, double>();

        [JsonProperty("timestamp")]
        public long LastUpdatedOn { get; set; }
    }
}
