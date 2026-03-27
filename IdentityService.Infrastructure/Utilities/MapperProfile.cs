using AutoMapper;
 using IdentityService.Domain.DTOs;
 using IdentityService.Domain.Forms;
 using IdentityService.Domain.Identity.DTOs;
 using IdentityService.Domain.Identity.Entities;
 using IdentityService.Domain.Identity.Forms;
 using IdentityService.Domain.Identity.Entities;

 namespace IdentityService.Infrastructure.Utilities;

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
     }
 }