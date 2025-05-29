using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class BudgetCategoryViewModel
    {
        public string Category { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
        public decimal PaidAmount { get; set; }
        public int ItemCount { get; set; }
        public decimal Percentage { get; set; }
    }
}
