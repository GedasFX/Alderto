using Alderto.Data.Models;
using AutoMapper;

namespace Alderto.Application.Features.Bank.Dto
{
    public class BankItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Quantity { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public BankItemDto(int id, string name, double value, double quantity, string? description, string? imageUrl)
        {
            Id = id;
            Name = name;
            Value = value;
            Quantity = quantity;
            Description = description;
            ImageUrl = imageUrl;
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildBankItem, BankItemDto>();
            }
        }
    }
}
