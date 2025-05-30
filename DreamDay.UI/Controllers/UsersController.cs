using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DreamDay.UI.Controllers
{
    [Route("users")]
    [Authorize(Policy = "AuthenticatedUsers")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly IWebHostEnvironment _env;
        public UsersController(IUserService userService, IFileHandlerService fileHandlerService, IWebHostEnvironment env)
        {
            _userService = userService;
            _fileHandlerService = fileHandlerService;
            _env = env;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var model = new UserViewModel();
            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string? filePath = null;
            if (vm.ImageFile != null)
            {
                try
                {
                    using var stream = vm.ImageFile.OpenReadStream();
                    filePath = await _fileHandlerService.SaveFileAsync(stream, vm.ImageFile.FileName);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(vm);
                }
            }

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = vm.Password,
                Role = string.IsNullOrEmpty(vm.Role) ? "User" : vm.Role,
                ImagePath = filePath,
                CreatedAt = DateTime.UtcNow
            };

            await _userService.CreateUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var vm = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Password = string.Empty, // Do not populate password in edit view for security
                ImagePath = user.ImagePath
            };
            return View(vm);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel vm)
        {
            if (id != vm.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (vm.ImageFile != null)
            {
                try
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrWhiteSpace(vm.ImagePath))
                    {
                        await _fileHandlerService.DeleteFileAsync(vm.ImagePath);
                    }

                    // Save new image
                    using var stream = vm.ImageFile.OpenReadStream();
                    user.ImagePath = await _fileHandlerService.SaveFileAsync(stream, vm.ImageFile.FileName);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(vm);
                }
            }

            user.FullName = vm.FullName;
            user.Email = vm.Email;
            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                user.Password = vm.Password;
            }
            user.Role = string.IsNullOrEmpty(vm.Role) ? user.Role : vm.Role;

            await _userService.UpdateUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null && !string.IsNullOrWhiteSpace(user.ImagePath))
            {
                try
                {
                    await _fileHandlerService.DeleteFileAsync(user.ImagePath);
                }
                catch (Exception)
                {
                    // Log the error but don't fail the deletion
                }
            }

            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
