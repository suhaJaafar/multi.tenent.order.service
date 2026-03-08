using AutoMapper;
 using MultiTenantOrderService.Domain.DTOs;
 using MultiTenantOrderService.Domain.Entities;
 using MultiTenantOrderService.Domain.Forms;
 using MultiTenantOrderService.Domain.Identity.DTOs;
 using MultiTenantOrderService.Domain.Identity.Entities;
 using MultiTenantOrderService.Domain.Identity.Forms;
 using MultiTenantOrderService.Domain.Identity.Entities;

 namespace MultiTenantOrderService.Infrastructure.Utilities;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<float?, float>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);


        CreateMap<UsersToReturnDto, User>().ReverseMap();
        CreateMap<CreateUserForm, User>();
        CreateMap<UpdateUserForm, User>()
            .ForMember(x => x.Password, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

        CreateMap<Product, ProductsToReturn>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt));
        CreateMap<CreateProductForm, Product>();

        CreateMap<CreateOrderForm, Order>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        CreateMap<Product, ProductDto>();
        
        // Map Order entity to OrderToReturn DTO - extract only IDs to avoid cycles
        CreateMap<Order, OrderToReturn>()
            .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        
        // Map Order to OrderInProductDto - used when orders are nested in products (no ProductIds needed)
        CreateMap<Order, OrderInProductDto>();
    }
}