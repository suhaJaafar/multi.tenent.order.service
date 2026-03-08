using AutoMapper;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Entities;
using MultiTenantOrderService.Domain.Identity.Entities;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.DTOs;
using MultiTenantOrderService.Domain.Identity.Forms;

namespace MultiTenantOrderService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map User entity to UserResponse (for GetUser query)
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Value : null))
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
            .ForMember(dest => dest.TenentName, opt => opt.MapFrom(src => src.TenentName.ToString()));

        // Map User entity to UsersToReturnDto
        CreateMap<User, UsersToReturnDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.TenentName, opt => opt.MapFrom(src => src.TenentName.ToString()));

        // Map CreateUserForm to User entity
        CreateMap<CreateUserForm, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.TenentName, opt => opt.MapFrom(src => src.TenentName));

        // Map Product entity to ProductsToReturn DTO
        CreateMap<Product, ProductsToReturn>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt));

        // Map CreateProductForm to Product entity
        CreateMap<CreateProductForm, Product>();
    }
}

