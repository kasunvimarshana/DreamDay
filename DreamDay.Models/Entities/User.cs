using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class User
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = "Guest"; // "Admin", "Planner", "Client", "Guest", etc.
        public string? ImagePath { get; set; } = string.Empty;
        public string? PasswordResetToken { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();
        public ICollection<PlannerWedding> PlannerWeddings { get; set; } = new List<PlannerWedding>();
        public User? CreatedBy { get; set; }
    }
}
