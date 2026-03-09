using AutoMapper;
using IdentityService.Application.GetUser;
using IdentityService.Domain.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Identity.Entities;
using IdentityService.Domain.Forms;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.DTOs;
using IdentityService.Domain.Identity.Forms;

namespace IdentityService.Application;

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

