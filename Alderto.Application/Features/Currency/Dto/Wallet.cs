using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto
{
    public static class Wallet
    {
        public class BriefDto
        {
            public int Amount { get; set; }
            public string Symbol { get; set; }

            public BriefDto(int amount, string symbol)
            {
                Amount = amount;
                Symbol = symbol;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.GuildMemberWallet, BriefDto>()
                    .ForMember(d => d.Symbol, o => o.MapFrom(s => s.Currency!.Symbol));
            }
        }
    }
}