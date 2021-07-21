using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto
{
    public class CurrencyTransactionDto
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public ulong SenderId { get; set; }
        public ulong RecipientId { get; set; }
        public int Amount { get; set; }
        public bool IsAward { get; set; }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.CurrencyTransaction, CurrencyTransactionDto>();
            }
        }
    }
}
