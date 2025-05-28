using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DreamDay.UI.Controllers
{
    [Route("vendors")]
    public class VendorController : Controller
    {
        private readonly IVendorService _vendorService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly IWebHostEnvironment _env;
        public VendorController(IVendorService vendorService, IFileHandlerService fileHandlerService, IWebHostEnvironment env)
        {
            _vendorService = vendorService;
            _fileHandlerService = fileHandlerService;
            _env = env;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var vendor = await _vendorService.GetAllVendorsAsync();
            return View(vendor);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            return vendor == null ? NotFound() : View(vendor);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? filePath = null;
            if (model.ImageFile != null)
            {
                try
                {
                    using var stream = model.ImageFile.OpenReadStream();
                    filePath = await _fileHandlerService.SaveFileAsync(stream, model.ImageFile.FileName);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(model);
                }
            }

            var vendor = new Vendor
            {
                Name = model.Name,
                Category = model.Category,
                Description = model.Description,
                ContactEmail = model.ContactEmail,
                ImagePath = filePath,
                CreatedAt = DateTime.UtcNow
            };

            await _vendorService.CreateVendorAsync(vendor);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            var model = new VendorViewModel
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Category = vendor.Category,
                Description = vendor.Description,
                ContactEmail = vendor.ContactEmail,
                ImagePath = vendor.ImagePath
            };
            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VendorViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var vendor = await _vendorService.GetVendorByIdAsync(model.Id);
            if (vendor == null)
            {
                return NotFound();
            }

            if (model.ImageFile != null)
            {
                try
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrWhiteSpace(vendor.ImagePath))
                    {
                        await _fileHandlerService.DeleteFileAsync(vendor.ImagePath);
                    }

                    // Save new image
                    using var stream = model.ImageFile.OpenReadStream();
                    vendor.ImagePath = await _fileHandlerService.SaveFileAsync(stream, model.ImageFile.FileName);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(model);
                }
            }

            vendor.Name = model.Name;
            vendor.Category = model.Category;
            vendor.Description = model.Description;
            vendor.ContactEmail = model.ContactEmail;

            await _vendorService.UpdateVendorAsync(vendor);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            return vendor == null ? NotFound() : View(vendor);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor != null && !string.IsNullOrWhiteSpace(vendor.ImagePath))
            {
                try
                {
                    await _fileHandlerService.DeleteFileAsync(vendor.ImagePath);
                }
                catch (Exception)
                {
                    // Log the error but don't fail the deletion
                }
            }

            await _vendorService.DeleteVendorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
