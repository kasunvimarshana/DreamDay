using DreamDay.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return View(dashboardData);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgetChartData()
        {
            var budgetData = await _dashboardService.GetBudgetByCategoryAsync();
            return Json(budgetData);
        }

        [HttpGet]
        public async Task<IActionResult> GetProgressChartData()
        {
            var progressData = await _dashboardService.GetWeddingProgressAsync();
            return Json(progressData);
        }
    }
}
