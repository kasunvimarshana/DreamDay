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
        public string RSVPStatus { get; set; } = "Pending"; // Pending, Accepted, Declined
        public bool IsVIP { get; set; }
        public string? DietaryRestrictions { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
        public int? PlusOne { get; set; } = 0; // Number of additional guests
        public string? TableAssignment { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RSVPDate { get; set; }
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
