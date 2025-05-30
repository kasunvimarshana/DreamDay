﻿using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Vendor
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty; // e.g., Photographer, Florist
        [MaxLength(1000)]
        public string? Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public ICollection<WeddingVendor> WeddingVendors { get; set; } = new List<WeddingVendor>();
        public User? CreatedBy { get; set; }
    }

}
