using IdentityService.Domain.Identity.DTOs;
using IdentityService.Domain.Identity.Entities;
using IdentityService.Domain.Identity.FiltersParams;
using IdentityService.Domain.Identity.Forms;

namespace IdentityService.Application.Abstractions;

public interface IUserService : IDisposable
{
    Task<ServiceResponse<List<UsersToReturnDto>>> GetUsers(UsersParams userParams);
    Task<ServiceResponse<User>> Login(string email, string password);
    Task<ServiceResponse<User>> GetUserById(Guid userId);
    Task<ServiceResponse<User>> AddUser(CreateUserForm form);
    Task<ServiceResponse<User>> UpdateUser(UpdateUserForm form);
}

