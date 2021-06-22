using System;
using AutoMapper;

namespace Alderto.Application.Features.Currency.Dto.Common
{
    public class GuidDto
    {
        public Guid Id { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
            }
        }
    }
}