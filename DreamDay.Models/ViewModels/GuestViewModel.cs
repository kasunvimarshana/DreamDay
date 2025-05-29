using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class GuestViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "RSVP Status")]
        public string RSVPStatus { get; set; } = "Pending";

        [Display(Name = "VIP Guest")]
        public bool IsVIP { get; set; }

        [Display(Name = "Dietary Restrictions")]
        public string? DietaryRestrictions { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Plus One Count")]
        [Range(0, 10, ErrorMessage = "Plus one count must be between 0 and 10")]
        public int? PlusOne { get; set; } = 0;

        [Display(Name = "Table Assignment")]
        public string? TableAssignment { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "RSVP Date")]
        public DateTime? RSVPDate { get; set; }

        [Required]
        public int WeddingId { get; set; }

        public List<(string Value, string Text)> RSVPStatusOptions { get; set; } = new()
        {
            ("Pending", "Pending"),
            ("Accepted", "Accepted"),
            ("Declined", "Declined")
        };
    }
}
