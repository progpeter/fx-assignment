using AutoMapper;
using KnabFX.Application.Models;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.Domain.Entities.Currency;

namespace KnabFX.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CryptoQuoteModel, CryptoQuote>();

            CreateMap<CurrencyRateModel, CurrencyRate>();
        }
    }
}
