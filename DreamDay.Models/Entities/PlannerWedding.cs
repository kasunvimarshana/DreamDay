using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    // Many-to-Many: A planner can manage many weddings
    public class PlannerWedding
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public int PlannerId { get; set; }
        [Required]
        public int WeddingId { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public Wedding Wedding { get; set; }
        public User Planner { get; set; }
        public User? CreatedBy { get; set; }
    }

}
