using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    [Route("venues")]
    public class VenueController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly IWebHostEnvironment _env;
        public VenueController(IVenueService venueService, IFileHandlerService fileHandlerService, IWebHostEnvironment env)
        {
            _venueService = venueService;
            _fileHandlerService = fileHandlerService;
            _env = env;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var venues = await _venueService.GetAllVenuesAsync();
            return View(venues);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            return venue == null ? NotFound() : View(venue);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueViewModel model)
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

            var venue = new Venue
            {
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                Price = model.Price,
                Description = model.Description,
                ImagePath = filePath
            };

            await _venueService.CreateVenueAsync(venue);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            var model = new VenueViewModel
            {
                Id = venue.Id,
                Name = venue.Name,
                Location = venue.Location,
                Capacity = venue.Capacity,
                Price = venue.Price,
                Description = venue.Description,
                ImagePath = venue.ImagePath
            };
            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VenueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var venue = await _venueService.GetVenueByIdAsync(model.Id);
            if (venue == null)
            {
                return NotFound();
            }

            if (model.ImageFile != null)
            {
                try
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrWhiteSpace(venue.ImagePath))
                    {
                        await _fileHandlerService.DeleteFileAsync(venue.ImagePath);
                    }

                    // Save new image
                    using var stream = model.ImageFile.OpenReadStream();
                    venue.ImagePath = await _fileHandlerService.SaveFileAsync(stream, model.ImageFile.FileName);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(model);
                }
            }

            venue.Name = model.Name;
            venue.Location = model.Location;
            venue.Capacity = model.Capacity;
            venue.Price = model.Price;
            venue.Description = model.Description;

            await _venueService.UpdateVenueAsync(venue);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            return venue == null ? NotFound() : View(venue);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue != null && !string.IsNullOrWhiteSpace(venue.ImagePath))
            {
                try
                {
                    await _fileHandlerService.DeleteFileAsync(venue.ImagePath);
                }
                catch (Exception)
                {
                    // Log the error but don't fail the deletion
                }
            }

            await _venueService.DeleteVenueAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
