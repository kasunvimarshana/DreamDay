using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;
        public string Role { get; set; }
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }

        public List<(string Value, string Text)> UserRoleOptions { get; set; } = new()
        {
            ("Admin", "Admin"),
            ("Guest", "Guest"),
            ("Planner", "Planner"),
            //("Client", "Client")
        };
    }
}
