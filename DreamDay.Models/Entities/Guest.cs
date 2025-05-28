using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Guest
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public bool RSVP { get; set; }
        public string? MealPreference { get; set; } = string.Empty;
        public bool IsAttending { get; set; }
        [Required]
        public int WeddingId { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public Wedding Wedding { get; set; }
        public User? CreatedBy { get; set; }
    }
}
