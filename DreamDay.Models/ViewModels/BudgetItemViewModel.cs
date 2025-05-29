using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class BudgetItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Item")]
        public string Item { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Estimated Cost")]
        public decimal EstimatedCost { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Actual Cost")]
        public decimal? ActualCost { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Required]
        public int WeddingId { get; set; }
    }
}
