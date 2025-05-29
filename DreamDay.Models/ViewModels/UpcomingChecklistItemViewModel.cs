using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class UpcomingChecklistItemViewModel
    {
        public int Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int WeddingId { get; set; }
        public string WeddingCouple { get; set; } = string.Empty;
        public int DaysUntilDue { get; set; }
        public bool IsOverdue { get; set; }
    }
}
