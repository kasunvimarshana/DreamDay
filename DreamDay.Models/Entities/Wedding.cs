using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Wedding
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? WeddingDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Tenant? Tenant { get; set; }
        public User User { get; set; }
        public ICollection<WeddingChecklistItem> WeddingChecklistItems { get; set; } = new List<WeddingChecklistItem>();
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    }

}
