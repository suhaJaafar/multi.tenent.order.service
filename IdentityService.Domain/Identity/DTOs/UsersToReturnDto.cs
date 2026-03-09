using IdentityService.Domain.Identity.Enums;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Identity.DTOs;
public class UsersToReturnDto
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public UserType UserType { get; set; }
    public required string TenentName { get; set; }
}