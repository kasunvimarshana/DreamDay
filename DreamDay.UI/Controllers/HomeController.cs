using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.ViewModels;
using DreamDay.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace DreamDay.UI.Controllers
{
    [Authorize(Policy = "AuthenticatedUsers")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();

            //if(User.IsInRole("Admin"))
            //{
            //    return View("AdminDashboard");
            //}

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
