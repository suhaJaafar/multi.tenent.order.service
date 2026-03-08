using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Identity.Forms;
public class LoginForm
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}