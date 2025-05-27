using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class VenueViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
    }
}
