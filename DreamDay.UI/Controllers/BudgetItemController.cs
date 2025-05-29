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
    [Route("weddings/{weddingId:int}/budget-items")]
    public class BudgetItemController : Controller
    {
        private readonly IBudgetItemService _budgetItemService;

        public BudgetItemController(IBudgetItemService budgetItemService)
        {
            _budgetItemService = budgetItemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int weddingId)
        {
            var budgetItems = await _budgetItemService.GetAllBudgetItemsByWeddingIdAsync(weddingId);
            ViewBag.WeddingId = weddingId;
            return View(budgetItems);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int weddingId, int id)
        {
            var budgetItem = await _budgetItemService.GetBudgetItemByIdAsync(id);

            if (budgetItem == null || budgetItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(budgetItem);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(int weddingId)
        {
            var model = new BudgetItemViewModel { WeddingId = weddingId };
            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int weddingId, BudgetItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var budgetItem = new BudgetItem
            {
                Category = model.Category,
                Item = model.Item,
                EstimatedCost = model.EstimatedCost,
                ActualCost = model.ActualCost,
                IsPaid = model.IsPaid,
                PaymentDate = model.PaymentDate,
                Notes = model.Notes,
                WeddingId = model.WeddingId,
                CreatedAt = DateTime.UtcNow
            };

            await _budgetItemService.CreateBudgetItemAsync(budgetItem);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int weddingId, int id)
        {
            var budgetItem = await _budgetItemService.GetBudgetItemByIdAsync(id);
            if (budgetItem == null || budgetItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            var model = new BudgetItemViewModel
            {
                Id = budgetItem.Id,
                Category = budgetItem.Category,
                Item = budgetItem.Item,
                EstimatedCost = budgetItem.EstimatedCost,
                ActualCost = budgetItem.ActualCost,
                IsPaid = budgetItem.IsPaid,
                PaymentDate = budgetItem.PaymentDate,
                Notes = budgetItem.Notes,
                WeddingId = budgetItem.WeddingId
            };

            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int weddingId, int id, BudgetItemViewModel model)
        {
            if (id != model.Id || weddingId != model.WeddingId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var budgetItem = await _budgetItemService.GetBudgetItemByIdAsync(model.Id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            budgetItem.Category = model.Category;
            budgetItem.Item = model.Item;
            budgetItem.EstimatedCost = model.EstimatedCost;
            budgetItem.ActualCost = model.ActualCost;
            budgetItem.IsPaid = model.IsPaid;
            budgetItem.PaymentDate = model.PaymentDate;
            budgetItem.Notes = model.Notes;

            await _budgetItemService.UpdateBudgetItemAsync(budgetItem);
            return RedirectToAction(nameof(Index), new { weddingId = model.WeddingId });
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int weddingId, int id)
        {
            var budgetItem = await _budgetItemService.GetBudgetItemByIdAsync(id);

            if (budgetItem == null || budgetItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            return View(budgetItem);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int weddingId, int id)
        {
            await _budgetItemService.DeleteBudgetItemAsync(id);
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }

        [HttpPost("toggle-paid/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePaid(int weddingId, int id)
        {
            var budgetItem = await _budgetItemService.GetBudgetItemByIdAsync(id);

            if (budgetItem == null || budgetItem.WeddingId != weddingId)
            {
                return NotFound();
            }

            budgetItem.IsPaid = !budgetItem.IsPaid;
            if (budgetItem.IsPaid && budgetItem.PaymentDate == null)
            {
                budgetItem.PaymentDate = DateTime.Now;
            }
            else if (!budgetItem.IsPaid)
            {
                budgetItem.PaymentDate = null;
            }

            await _budgetItemService.UpdateBudgetItemAsync(budgetItem);
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }
    }
}
