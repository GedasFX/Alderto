using System.Collections.Generic;
using Alderto.Data.Models.GuildBank;
using AutoMapper;

namespace Alderto.Application.Features.Bank.Dto
{
    public class BankDto : BankBriefDto
    {
        public ICollection<BankItemDto> Contents { get; set; }

        public BankDto(int id, string name) : base(id, name)
        {
            Id = id;
            Name = name;
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildBank, BankDto>();
            }
        }
    }
}
