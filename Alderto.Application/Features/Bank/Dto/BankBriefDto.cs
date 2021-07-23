using Alderto.Data.Models;
using AutoMapper;

namespace Alderto.Application.Features.Bank.Dto
{
    public class BankBriefDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BankBriefDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildBank, BankBriefDto>();
            }
        }
    }
}
