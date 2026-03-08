using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Forms;
public class UpdateUserForm : CreateUserForm
{
    [Required]
    public Guid Id { get; set; }
}