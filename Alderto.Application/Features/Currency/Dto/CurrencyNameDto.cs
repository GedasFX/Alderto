using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto
{
    public class CurrencyNameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public CurrencyNameDto(Guid id, string name, string symbol)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, CurrencyNameDto>();
            }
        }
    }
}
