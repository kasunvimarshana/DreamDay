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
        public string Title { get; set; } = string.Empty;
        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedAmount { get; set; } = 0.00m;
        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0.00m;
        [Required]
        public int WeddingId { get; set; }
        [DataType(DataType.Date)]
        public int? CreatedByUserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        public Tenant? Tenant { get; set; }
        public Wedding Wedding { get; set; }
        public User? CreatedBy { get; set; }
    }

}
