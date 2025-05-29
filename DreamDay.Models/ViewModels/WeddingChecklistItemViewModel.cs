using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingChecklistItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Task")]
        public string Task { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; }

        [Required]
        public int WeddingId { get; set; }
    }
}
