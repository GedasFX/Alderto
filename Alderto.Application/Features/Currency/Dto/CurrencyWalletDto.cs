using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto
{
    public class CurrencyWalletDto
    {
        public Guid Id { get; set; }
        public ulong MemberId { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset TimelyLastClaimed { get; set; }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.GuildMemberWallet, CurrencyWalletDto>();
            }
        }
    }
}
