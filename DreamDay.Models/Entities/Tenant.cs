using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; // e.g., "Elegant Weddings Inc."
        public string? Domain { get; set; } = string.Empty; // optional for custom domain support
        public bool IsActive { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<User> Users { get; set; }
    }
}
