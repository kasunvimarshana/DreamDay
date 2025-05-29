using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalWeddings { get; set; }
        public int UpcomingWeddings { get; set; }
        public int TotalVenues { get; set; }
        public int TotalUsers { get; set; }
        public List<Wedding> RecentWeddings { get; set; }
    }

}
