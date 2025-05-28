using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    // Many-to-Many: Each wedding can have multiple vendors
    public class WeddingVendor
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public int WeddingId { get; set; }
        [Required]
        public int VendorId { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public Wedding Wedding { get; set; }
        public Vendor Vendor { get; set; }
        public User? CreatedBy { get; set; }
    }

}
