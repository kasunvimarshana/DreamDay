using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalWeddings { get; set; }
        public int UpcomingWeddings { get; set; }
        public int PastWeddings { get; set; }
        public int TotalChecklistItems { get; set; }
        public int CompletedChecklistItems { get; set; }
        public int PendingChecklistItems { get; set; }
        public int OverdueChecklistItems { get; set; }
        public decimal TotalBudgetEstimated { get; set; }
        public decimal TotalBudgetActual { get; set; }
        public decimal TotalBudgetPaid { get; set; }
        public decimal BudgetVariance { get; set; }
        public int TotalBudgetItems { get; set; }
        public int PaidBudgetItems { get; set; }
        public int UnpaidBudgetItems { get; set; }

        public List<RecentWeddingViewModel> RecentWeddings { get; set; } = new();
        public List<UpcomingChecklistItemViewModel> UpcomingTasks { get; set; } = new();
        public List<BudgetCategoryViewModel> BudgetByCategory { get; set; } = new();
        public List<MonthlyBudgetViewModel> MonthlyBudgetTrend { get; set; } = new();
        public List<WeddingProgressViewModel> WeddingProgress { get; set; } = new();
    }

}
