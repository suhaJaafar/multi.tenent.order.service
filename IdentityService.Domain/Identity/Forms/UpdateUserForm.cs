using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Identity.Forms;
public class UpdateUserForm : CreateUserForm
{
    [Required]
    public Guid Id { get; set; }
}