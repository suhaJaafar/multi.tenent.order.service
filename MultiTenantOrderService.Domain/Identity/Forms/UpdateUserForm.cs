using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Identity.Forms;
public class UpdateUserForm : CreateUserForm
{
    [Required]
    public Guid Id { get; set; }
}