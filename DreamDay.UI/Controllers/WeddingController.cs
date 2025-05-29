using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace DreamDay.UI.Controllers
{
    [Route("weddings")]
    public class WeddingController : Controller
    {
        private readonly IWeddingService _weddingService;
        private readonly IVenueService _venueService;
        private readonly IUserService _userService;

        public WeddingController(
            IWeddingService weddingService, 
            IVenueService venueService, 
            IUserService userService
        )
        {
            _weddingService = weddingService;
            _venueService = venueService;
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var weddings = await _weddingService.GetAllWeddingsAsync();
            return View(weddings);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var wedding = await _weddingService.GetWeddingByIdAsync(id);
            return wedding == null ? NotFound() : View(wedding);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await PopulateSelectLists();
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeddingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectLists();
                return View(model);
            }

            var wedding = new Wedding
            {
                Title = model.Title,
                WeddingDate = model.WeddingDate,
                //OwnerId = model.OwnerId,
                BrideId = model.BrideId,
                GroomId = model.GroomId,
                VenueId = model.VenueId,
                CreatedAt = DateTime.UtcNow
            };

            await _weddingService.CreateWeddingAsync(wedding);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var wedding = await _weddingService.GetWeddingByIdAsync(id);
            if (wedding == null)
            {
                return NotFound();
            }

            var model = new WeddingViewModel
            {
                Id = wedding.Id,
                Title = wedding.Title,
                WeddingDate = (DateTime) wedding.WeddingDate,
                //OwnerId = (int) wedding.OwnerId,
                BrideId = (int) wedding.BrideId,
                GroomId = (int) wedding.GroomId,
                VenueId = (int) wedding.VenueId
            };

            await PopulateSelectLists();
            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WeddingViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectLists();
                return View(model);
            }

            var wedding = await _weddingService.GetWeddingByIdAsync(model.Id);
            if (wedding == null)
            {
                return NotFound();
            }

            wedding.Title = model.Title;
            wedding.WeddingDate = model.WeddingDate;
            //wedding.OwnerId = model.OwnerId;
            wedding.BrideId = model.BrideId;
            wedding.GroomId = model.GroomId;
            wedding.VenueId = model.VenueId;

            await _weddingService.UpdateWeddingAsync(wedding);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wedding = await _weddingService.GetWeddingByIdAsync(id);
            return wedding == null ? NotFound() : View(wedding);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _weddingService.DeleteWeddingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelectLists()
        {
            var venues = await _venueService.GetAllVenuesAsync();
            var owners = await _userService.GetAllUsersAsync();

            ViewBag.VenueId = new SelectList(venues, "Id", "Name");
            ViewBag.UserId = new SelectList(owners, "Id", "FullName");
        }
    }
}
