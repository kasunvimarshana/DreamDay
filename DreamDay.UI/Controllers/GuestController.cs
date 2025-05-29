using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace DreamDay.UI.Controllers
{
    [Route("weddings/{weddingId:int}/guests")]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int weddingId)
        {
            var guests = await _guestService.GetAllGuestsByWeddingIdAsync(weddingId);
            var rsvpStats = await _guestService.GetRSVPStatsAsync(weddingId);
            var totalGuestCount = await _guestService.GetTotalGuestCountAsync(weddingId);

            ViewBag.WeddingId = weddingId;
            ViewBag.RSVPStats = rsvpStats;
            ViewBag.TotalGuestCount = totalGuestCount;

            return View(guests);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int weddingId, int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);

            if (guest == null || guest.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(int weddingId)
        {
            var model = new GuestViewModel { WeddingId = weddingId };
            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int weddingId, GuestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var guest = new Guest
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                RSVPStatus = model.RSVPStatus,
                IsVIP = model.IsVIP,
                DietaryRestrictions = model.DietaryRestrictions,
                Notes = model.Notes,
                PlusOne = model.PlusOne,
                TableAssignment = model.TableAssignment,
                RSVPDate = model.RSVPDate,
                WeddingId = model.WeddingId,
                CreatedAt = DateTime.UtcNow
            };

            await _guestService.CreateGuestAsync(guest);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int weddingId, int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            if (guest == null || guest.WeddingId != weddingId)
            {
                return NotFound();
            }

            var model = new GuestViewModel
            {
                Id = guest.Id,
                FullName = guest.FullName,
                Email = guest.Email,
                PhoneNumber = guest.PhoneNumber,
                RSVPStatus = guest.RSVPStatus,
                IsVIP = guest.IsVIP,
                DietaryRestrictions = guest.DietaryRestrictions,
                Notes = guest.Notes,
                PlusOne = guest.PlusOne,
                TableAssignment = guest.TableAssignment,
                RSVPDate = guest.RSVPDate,
                WeddingId = guest.WeddingId
            };

            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int weddingId, int id, GuestViewModel model)
        {
            if (id != model.Id || weddingId != model.WeddingId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var guest = await _guestService.GetGuestByIdAsync(model.Id);
            if (guest == null)
            {
                return NotFound();
            }

            guest.FullName = model.FullName;
            guest.Email = model.Email;
            guest.PhoneNumber = model.PhoneNumber;
            guest.RSVPStatus = model.RSVPStatus;
            guest.IsVIP = model.IsVIP;
            guest.DietaryRestrictions = model.DietaryRestrictions;
            guest.Notes = model.Notes;
            guest.PlusOne = model.PlusOne;
            guest.TableAssignment = model.TableAssignment;
            guest.RSVPDate = model.RSVPDate;

            await _guestService.UpdateGuestAsync(guest);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int weddingId, int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);

            if (guest == null || guest.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int weddingId, int id)
        {
            await _guestService.DeleteGuestAsync(id);
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }

        [HttpPost("update-rsvp/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRSVP(int weddingId, int id, string status)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);

            if (guest == null || guest.WeddingId != weddingId)
            {
                return NotFound();
            }

            guest.RSVPStatus = status;
            if (status != "Pending")
            {
                guest.RSVPDate = DateTime.Now;
            }

            await _guestService.UpdateGuestAsync(guest);
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }
    }
}
