using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IWeddingService _weddingService;
        private readonly IWeddingChecklistItemService _checklistService;
        private readonly IBudgetItemService _budgetService;

        public DashboardService(
            IWeddingService weddingService,
            IWeddingChecklistItemService checklistService,
            IBudgetItemService budgetService)
        {
            _weddingService = weddingService;
            _checklistService = checklistService;
            _budgetService = budgetService;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(int? tenantId = null, int? userId = null)
        {
            var weddings = await _weddingService.GetAllWeddingsAsync();
            var checklistItems = await _checklistService.GetAllWeddingChecklistItemsAsync();
            var budgetItems = await _budgetService.GetAllBudgetItemsAsync();

            var now = DateTime.Now;

            var dashboard = new DashboardViewModel
            {
                // Wedding Statistics
                TotalWeddings = weddings.Count(),
                UpcomingWeddings = weddings.Count(w => w.WeddingDate > now),
                PastWeddings = weddings.Count(w => w.WeddingDate <= now),

                // Checklist Statistics
                TotalChecklistItems = checklistItems.Count(),
                CompletedChecklistItems = checklistItems.Count(c => c.IsCompleted),
                PendingChecklistItems = checklistItems.Count(c => !c.IsCompleted),
                OverdueChecklistItems = checklistItems.Count(c => !c.IsCompleted && c.DueDate.HasValue && c.DueDate.Value < now),

                // Budget Statistics
                TotalBudgetEstimated = budgetItems.Sum(b => b.EstimatedCost),
                TotalBudgetActual = budgetItems.Where(b => b.ActualCost.HasValue).Sum(b => b.ActualCost.Value),
                TotalBudgetPaid = budgetItems.Where(b => b.IsPaid && b.ActualCost.HasValue).Sum(b => b.ActualCost.Value),
                TotalBudgetItems = budgetItems.Count(),
                PaidBudgetItems = budgetItems.Count(b => b.IsPaid),
                UnpaidBudgetItems = budgetItems.Count(b => !b.IsPaid),

                // Complex Data
                RecentWeddings = await GetRecentWeddingsAsync(tenantId, userId),
                UpcomingTasks = await GetUpcomingTasksAsync(tenantId, userId),
                BudgetByCategory = await GetBudgetByCategoryAsync(tenantId, userId),
                WeddingProgress = await GetWeddingProgressAsync(tenantId, userId)
            };

            dashboard.BudgetVariance = dashboard.TotalBudgetActual - dashboard.TotalBudgetEstimated;

            return dashboard;
        }

        public async Task<List<RecentWeddingViewModel>> GetRecentWeddingsAsync(int? tenantId = null, int? userId = null)
        {
            var weddings = await _weddingService.GetAllWeddingsAsync();
            var checklistItems = await _checklistService.GetAllWeddingChecklistItemsAsync();
            var budgetItems = await _budgetService.GetAllBudgetItemsAsync();

            return weddings.OrderByDescending(w => w.WeddingDate)
                .Take(5)
                .Select(w =>
                {
                    var weddingTasks = checklistItems.Where(c => c.WeddingId == w.Id);
                    var weddingBudget = budgetItems.Where(b => b.WeddingId == w.Id);

                    var daysUntil = w.WeddingDate.HasValue ?
                        (int)(w.WeddingDate.Value - DateTime.Now).TotalDays : 0;

                    return new RecentWeddingViewModel
                    {
                        Id = w.Id,
                        BrideName = w.Bride?.FullName,
                        GroomName = w.Groom?.FullName,
                        WeddingDate = w.WeddingDate ?? DateTime.MinValue,
                        Venue = w.Venue?.Name ?? "TBD",
                        DaysUntilWedding = daysUntil,
                        CompletedTasks = weddingTasks.Count(t => t.IsCompleted),
                        TotalTasks = weddingTasks.Count(),
                        EstimatedBudget = weddingBudget.Sum(b => b.EstimatedCost),
                        ActualBudget = weddingBudget.Where(b => b.ActualCost.HasValue).Sum(b => b.ActualCost.Value)
                    };
                }).ToList();
        }

        public async Task<List<UpcomingChecklistItemViewModel>> GetUpcomingTasksAsync(int? tenantId = null, int? userId = null)
        {
            var checklistItems = await _checklistService.GetAllWeddingChecklistItemsAsync();
            var weddings = await _weddingService.GetAllWeddingsAsync();
            var now = DateTime.Now;

            return checklistItems
                .Where(c => !c.IsCompleted && c.DueDate.HasValue && c.DueDate.Value >= now)
                .OrderBy(c => c.DueDate)
                .Take(10)
                .Select(c =>
                {
                    var wedding = weddings.FirstOrDefault(w => w.Id == c.WeddingId);

                    var daysUntil = c.DueDate.HasValue ?
                        (int)(c.DueDate.Value - now).TotalDays : 0;

                    return new UpcomingChecklistItemViewModel
                    {
                        Id = c.Id,
                        Task = c.Task,
                        DueDate = c.DueDate,
                        WeddingId = c.WeddingId,
                        WeddingCouple = wedding != null ? $"{wedding.Bride?.FullName} & {wedding.Groom?.FullName}" : "Unknown",
                        DaysUntilDue = daysUntil,
                        IsOverdue = c.DueDate.HasValue && c.DueDate.Value < now
                    };
                }).ToList();
        }

        public async Task<List<BudgetCategoryViewModel>> GetBudgetByCategoryAsync(int? tenantId = null, int? userId = null)
        {
            var budgetItems = await _budgetService.GetAllBudgetItemsAsync();
            var totalEstimated = budgetItems.Sum(b => b.EstimatedCost);

            return budgetItems
                .GroupBy(b => b.Category)
                .Select(g => new BudgetCategoryViewModel
                {
                    Category = g.Key,
                    EstimatedCost = g.Sum(b => b.EstimatedCost),
                    ActualCost = g.Where(b => b.ActualCost.HasValue).Sum(b => b.ActualCost.Value),
                    PaidAmount = g.Where(b => b.IsPaid && b.ActualCost.HasValue).Sum(b => b.ActualCost.Value),
                    ItemCount = g.Count(),
                    Percentage = totalEstimated > 0 ? (g.Sum(b => b.EstimatedCost) / totalEstimated) * 100 : 0
                })
                .OrderByDescending(c => c.EstimatedCost)
                .ToList();
        }

        public async Task<List<WeddingProgressViewModel>> GetWeddingProgressAsync(int? tenantId = null, int? userId = null)
        {
            var weddings = await _weddingService.GetAllWeddingsAsync();
            var checklistItems = await _checklistService.GetAllWeddingChecklistItemsAsync();
            var now = DateTime.Now;

            return weddings
                .Where(w => w.WeddingDate.HasValue && w.WeddingDate.Value > now)
                .Select(w =>
                {
                    var weddingTasks = checklistItems.Where(c => c.WeddingId == w.Id);
                    var totalTasks = weddingTasks.Count();
                    var completedTasks = weddingTasks.Count(t => t.IsCompleted);
                    var progress = totalTasks > 0 ? (decimal)completedTasks / totalTasks * 100 : 0;

                    var daysRemaining = w.WeddingDate.HasValue ?
                        (int)(w.WeddingDate.Value - now).TotalDays : 0;

                    return new WeddingProgressViewModel
                    {
                        WeddingId = w.Id,
                        CoupleName = $"{w.Bride?.FullName} & {w.Groom?.FullName}",
                        WeddingDate = w.WeddingDate ?? DateTime.MinValue,
                        TotalTasks = totalTasks,
                        CompletedTasks = completedTasks,
                        ProgressPercentage = progress,
                        DaysRemaining = daysRemaining
                    };
                })
                .OrderBy(w => w.WeddingDate)
                .ToList();
        }
    }
}