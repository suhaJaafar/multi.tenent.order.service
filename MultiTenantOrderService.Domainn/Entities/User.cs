using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.ObjectValues;

namespace MultiTenantOrderService.Domain.Entities;

public class User : BaseModel
{
    public PhoneNumber? PhoneNumber { get; set; }
    public Email Email { get; set; }
    public string Name { get; set; }
    public Password Password { get; set; }
    public UserType UserType { get; set; }
    
    public TenentName TenentName { get; set; }
}