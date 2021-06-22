using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto
{
    public static class Currency
    {
        public class Dto
        {
            public Guid Id { get; }
            public string Symbol { get; }
            public string Name { get; }
            public string? Description { get; }
            public int? TimelyInterval { get; }
            public int TimelyAmount { get; }

            public Dto(Guid id, string symbol, string name, string? description = null,
                int? timelyInterval = null, int timelyAmount = 0)
            {
                Id = id;
                Symbol = symbol;
                Name = name;
                Description = description;
                TimelyInterval = timelyInterval;
                TimelyAmount = timelyAmount;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Dto>();
            }
        }
    }
}