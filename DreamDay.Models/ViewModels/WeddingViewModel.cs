using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Wedding Title")]
        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date")]
        public DateTime WeddingDate { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        //[Required]
        //[Display(Name = "Owner")]
        //public int OwnerId { get; set; }

        [Required]
        [Display(Name = "Bride")]
        public int BrideId { get; set; }

        [Required]
        [Display(Name = "Groom")]
        public int GroomId { get; set; }
    }
}
