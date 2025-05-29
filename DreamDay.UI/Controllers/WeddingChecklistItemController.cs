using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Collections.Specialized.BitVector32;

namespace DreamDay.UI.Controllers
{
    [Route("weddings/{weddingId:int}/wedding-checklist-items")]
    public class WeddingChecklistItemController : Controller
    {
        private readonly IWeddingChecklistItemService _weddingChecklistItemService;

        public WeddingChecklistItemController(
            IWeddingChecklistItemService weddingChecklistItemService
        )
        {
            _weddingChecklistItemService = weddingChecklistItemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int weddingId)
        {
            var weddingChecklistItems = await _weddingChecklistItemService.GetAllWeddingChecklistItemsByWeddingIdAsync(weddingId);
            ViewBag.WeddingId = weddingId;
            return View(weddingChecklistItems);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int weddingId, int id)
        {
            var weddingChecklistItem = await _weddingChecklistItemService.GetWeddingChecklistItemByIdAsync(id);

            if (weddingChecklistItem == null || weddingChecklistItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(weddingChecklistItem);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(int weddingId)
        {
            var model = new WeddingChecklistItemViewModel { WeddingId = weddingId };
            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int weddingId, WeddingChecklistItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var weddingChecklistItem = new WeddingChecklistItem
            {
                Task = model.Task,
                DueDate = model.DueDate,
                IsCompleted = model.IsCompleted,
                WeddingId = model.WeddingId,
                CreatedAt = DateTime.UtcNow
            };

            await _weddingChecklistItemService.CreateWeddingChecklistItemAsync(weddingChecklistItem);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int weddingId, int id)
        {
            var weddingChecklistItem = await _weddingChecklistItemService.GetWeddingChecklistItemByIdAsync(id);
            if (weddingChecklistItem == null || weddingChecklistItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            var model = new WeddingChecklistItemViewModel
            {
                Id = weddingChecklistItem.Id,
                Task = weddingChecklistItem.Task,
                DueDate = (DateTime)weddingChecklistItem.DueDate,
                IsCompleted = weddingChecklistItem.IsCompleted,
                WeddingId = weddingChecklistItem.WeddingId
            };

            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int weddingId, int id, WeddingChecklistItemViewModel model)
        {
            if (id != model.Id || weddingId != model.WeddingId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var weddingChecklistItem = await _weddingChecklistItemService.GetWeddingChecklistItemByIdAsync(model.Id);
            if (weddingChecklistItem == null)
            {
                return NotFound();
            }

            weddingChecklistItem.Task = model.Task;
            weddingChecklistItem.DueDate = model.DueDate;
            weddingChecklistItem.IsCompleted = model.IsCompleted;

            await _weddingChecklistItemService.UpdateWeddingChecklistItemAsync(weddingChecklistItem);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int weddingId, int id)
        {
            var weddingChecklistItem = await _weddingChecklistItemService.GetWeddingChecklistItemByIdAsync(id);

            if (weddingChecklistItem == null || weddingChecklistItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(weddingChecklistItem);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int weddingId, int id)
        {
            await _weddingChecklistItemService.DeleteWeddingChecklistItemAsync(id);
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }

        [HttpPost("toggle-completed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCompleted(int weddingId, int id)
        {
            var weddingChecklistItem = await _weddingChecklistItemService.GetWeddingChecklistItemByIdAsync(id);

            if (weddingChecklistItem == null || weddingChecklistItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            weddingChecklistItem.IsCompleted = !weddingChecklistItem.IsCompleted;
            await _weddingChecklistItemService.UpdateWeddingChecklistItemAsync(weddingChecklistItem);

            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }
    }
}
