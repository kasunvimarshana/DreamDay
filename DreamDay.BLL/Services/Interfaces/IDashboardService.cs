using DreamDay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync(int? tenantId = null, int? userId = null);
        Task<List<RecentWeddingViewModel>> GetRecentWeddingsAsync(int? tenantId = null, int? userId = null);
        Task<List<UpcomingChecklistItemViewModel>> GetUpcomingTasksAsync(int? tenantId = null, int? userId = null);
        Task<List<BudgetCategoryViewModel>> GetBudgetByCategoryAsync(int? tenantId = null, int? userId = null);
        Task<List<WeddingProgressViewModel>> GetWeddingProgressAsync(int? tenantId = null, int? userId = null);
    }
}
