using AutoMapper;
using CatalogService.Application.GetProduct;
using CatalogService.Domain.Product.DTOs;
using CatalogService.Domain.Product.Entities;

namespace CatalogService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Product entity to ProductResponse
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

        // Map Product entity to ProductToReturnDto
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
    }
}