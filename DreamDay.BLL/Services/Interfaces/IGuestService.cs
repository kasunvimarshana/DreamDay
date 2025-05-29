using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuestsAsync();
        Task<Guest?> GetGuestByIdAsync(int id);
        Task CreateGuestAsync(Guest guest);
        Task UpdateGuestAsync(Guest guest);
        Task DeleteGuestAsync(int id);
        Task<IEnumerable<Guest>> GetAllGuestsByWeddingIdAsync(int weddingId);
        Task<Dictionary<string, int>> GetRSVPStatsAsync(int weddingId);
        Task<int> GetTotalGuestCountAsync(int weddingId);
    }
}
