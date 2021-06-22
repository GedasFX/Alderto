using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto.Common
{
    public class IdDto
    {
        public int Id { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
            }
        }
    }
}