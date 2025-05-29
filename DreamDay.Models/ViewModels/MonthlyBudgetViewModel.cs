using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class MonthlyBudgetViewModel
    {
        public string Month { get; set; } = string.Empty;
        public decimal EstimatedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
