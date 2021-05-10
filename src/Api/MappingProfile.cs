using ApplicationCore.Entities;
using AutoMapper;
using PublicApi.Dtos;

namespace PublicApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Conversion, ConversionDto>();
        }
    }
}
