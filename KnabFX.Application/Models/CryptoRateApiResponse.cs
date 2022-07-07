using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnabFX.Application.Models
{
    public class CryptoRateApiResponse
    {
        public Status Status { get; set; }
        public JToken Data { get; set; }

        //[JsonProperty("data")]
        //public Dictionary<string, CryptoQuoteModel> SandboxData { get; set; }

        //[JsonProperty("data")]
        //public List<CryptoQuoteModel> LiveData { get; set; }
    }
    //public class CryptoRateApiSandboxResponse
    //{
    //    public Dictionary<string, CryptoQuoteModel> Data { get; set; }
    //}

    //public class CryptoRateApiLiveResponse
    //{
    //    public List<CryptoQuoteModel> Data { get; set; }
    //}

    public class Status
    {
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("credit_count")]
        public int CreditCount { get; set; }
    }
}
