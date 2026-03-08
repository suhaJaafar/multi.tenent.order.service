using System.ComponentModel.DataAnnotations;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Domain.Identity.Forms;
public class CreateUserForm
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public UserType UserType { get; set; }
    
    [Required]
    public TenentName TenentName { get; set; }

}
