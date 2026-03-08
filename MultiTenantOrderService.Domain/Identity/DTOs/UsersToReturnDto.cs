using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Domain.Identity.DTOs;
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