using AutoMapper;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Entities;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Domain.Enums;

namespace MultiTenantOrderService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map User entity to UsersToReturnDto
        CreateMap<User, UsersToReturnDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.TenentName, opt => opt.MapFrom(src => src.TenentName.ToString()));

        // Map CreateUserForm to User entity
        CreateMap<CreateUserForm, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.TenentName, opt => opt.MapFrom(src => src.TenentName));
    }
}

