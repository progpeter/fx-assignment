namespace KnabFX.Infrastructure.Common
{
    public class ExternalServiceBaseConfig
    {
        public string BaseAddress { get; set; }

        public string DefaultApi { get; set; }

        public Dictionary<string, string> Headers { get; set; } = new();
    }
}
