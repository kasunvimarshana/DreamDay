using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class RecentWeddingViewModel
    {
        public int Id { get; set; }
        public string BrideName { get; set; } = string.Empty;
        public string GroomName { get; set; } = string.Empty;
        public DateTime WeddingDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int DaysUntilWedding { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal ActualBudget { get; set; }
        public bool IsUpcoming => WeddingDate > DateTime.Now;
    }
}
