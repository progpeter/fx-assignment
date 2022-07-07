namespace KnabFX.Domain.Entities.Cryptocurrency
{
    public class CryptoQuote
    {
        public string Symbol { get; set; } 
        public string Name { get; set; }   
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedOnSeconds
        {
            get
            {
                return new DateTimeOffset(this.LastUpdatedOn).ToUnixTimeSeconds();
            }
        }
        public Dictionary<string, CryptoQuotePrice> Quote { get; set; } = new Dictionary<string, CryptoQuotePrice>();
    }

   
}
