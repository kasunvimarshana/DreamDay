using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class BudgetItem
    {
        public int? TenantId { get; set; }
        public int Id { get; set; }

        [Required]
        public string Item { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedCost { get; set; } = 0.00m;
        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualCost { get; set; } = 0.00m;
        public bool IsPaid { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
        [Required]
        public int WeddingId { get; set; }
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public Wedding Wedding { get; set; }
        public User? CreatedBy { get; set; }
    }

}
