using ApplicationCore.Entities;
using AutoMapper;
using PublicApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Conversion, ConversionDto>();
        }
    }
}
