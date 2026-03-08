using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantOrderService.Domain.Entities
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
