using IdentityService.Infrastructure.Utilities;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Entities;
using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Infrastructure.Utilities;

namespace MultiTenantOrderService.Infrastructure.Interfaces;
public interface IUserService : IDisposable
{
    Task<ServiceResponse<List<UsersToReturnDto>>> GetUsers(UsersParams userParams);
    Task<ServiceResponse<User>> Login(string email, string password);
    Task<ServiceResponse<User> > GetUserById(Guid userId);
    Task<ServiceResponse<User>> AddUser(CreateUserForm form);
    Task<ServiceResponse<User>> UpdateUser(UpdateUserForm form);
}