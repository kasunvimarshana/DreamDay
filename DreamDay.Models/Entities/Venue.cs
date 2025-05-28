using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DreamDay.Models.Entities
{
    public class Venue
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0.00m;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Tenant? Tenant { get; set; }
    }

}
