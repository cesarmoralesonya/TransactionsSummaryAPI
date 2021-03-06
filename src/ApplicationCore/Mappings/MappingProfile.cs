using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Models;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionModel, TransactionDto>();
            CreateMap<TransactionEntity, TransactionDto>()
                .ForMember(d => d.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(src => src.Currency));
            CreateMap<TransactionModel, TransactionEntity>()
                .ForMember(d => d.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(src => src.Currency));

            CreateMap<RateModel, RateDto>();
            CreateMap<RateEntity, RateDto>()
                .ForMember(d => d.From, opt => opt.MapFrom(src => src.From))
                .ForMember(d => d.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(d => d.To, opt => opt.MapFrom(src => src.To));
            CreateMap<RateModel, RateEntity>()
                .ForMember(d => d.From, opt => opt.MapFrom(src => src.From))
                .ForMember(d => d.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(d => d.To, opt => opt.MapFrom(src => src.To));
        }
    }
}
