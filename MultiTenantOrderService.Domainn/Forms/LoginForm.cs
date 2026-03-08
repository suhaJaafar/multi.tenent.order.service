using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Forms;
public class LoginForm
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}