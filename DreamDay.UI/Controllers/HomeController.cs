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
    public class HomeController : Controller
    {
        private readonly IWeddingService _weddingService;
        private readonly IVenueService _venueService;
        private readonly IUserService _userService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWeddingService weddingService, IVenueService venueService, IUserService userService)
        {
            _logger = logger;
            _weddingService = weddingService;
            _venueService = venueService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var weddings = await _weddingService.GetAllWeddingsAsync();
            var upcomingWeddings = weddings.Where(w => w.WeddingDate >= DateTime.Today).ToList();
            var recentWeddings = weddings.OrderByDescending(w => w.CreatedAt).Take(5).ToList();

            var model = new DashboardViewModel
            {
                TotalWeddings = weddings.Count(),
                UpcomingWeddings = upcomingWeddings.Count,
                TotalVenues = (await _venueService.GetAllVenuesAsync()).Count(),
                TotalUsers = (await _userService.GetAllUsersAsync()).Count(),
                RecentWeddings = recentWeddings
            };

            return View(model);
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
