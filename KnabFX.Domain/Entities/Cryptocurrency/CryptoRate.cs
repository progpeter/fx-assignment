namespace KnabFX.Domain.Entities.Cryptocurrency
{
    public record CryptoRate
    {
        public CryptoRate(string symbol, string exCurrency, double price, DateTimeOffset lastUpdatedOn)
        {
            Symbol = symbol;
            ExCurrency = exCurrency;
            Price = price;
            LastUpdatedOn = lastUpdatedOn;
        }

        public string Symbol { get; set; }
        public string ExCurrency { get; set; }
        public double Price { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }
    }
}
