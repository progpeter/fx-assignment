namespace KnabFX.Domain.Entities.Currency
{
    public record CurrencyRate
    {
        public string BaseCurrency { get; set; }
        
        public Dictionary<string,double> Rates { get; set; } = new Dictionary<string, double>();

        public long LastUpdatedOn { get; set; }
    }
}
