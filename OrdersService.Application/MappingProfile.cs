using AutoMapper;
using OrdersService.Application.CreateOrder;
using OrdersService.Domain.Order.Entities;

namespace OrdersService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}

