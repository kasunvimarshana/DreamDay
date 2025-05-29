using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Wedding
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? WeddingDate { get; set; }
        //public string? Location { get; set; } = string.Empty;
        //public int? OwnerId { get; set; }
        public int? BrideId { get; set; }
        public int? GroomId { get; set; }
        public int? VenueId { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        //public User? Owner { get; set; }
        public User? Bride { get; set; }
        public User? Groom { get; set; }
        public Venue? Venue { get; set; }
        public ICollection<WeddingChecklistItem> WeddingChecklistItems { get; set; } = new List<WeddingChecklistItem>();
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();
        public ICollection<TimelineEvent> TimelineEvents { get; set; } = new List<TimelineEvent>();
        public ICollection<WeddingVendor> WeddingVendors { get; set; } = new List<WeddingVendor>();
        public ICollection<PlannerWedding> PlannerWeddings { get; set; } = new List<PlannerWedding>();

        public User? CreatedBy { get; set; }
    }

}
