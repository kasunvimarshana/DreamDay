using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public Task<IEnumerable<Guest>> GetAllGuestsAsync() => _guestRepository.GetAllAsync();

        public Task<Guest?> GetGuestByIdAsync(int id) => _guestRepository.GetByIdAsync(id);

        public Task CreateGuestAsync(Guest guest)
        {
            return _guestRepository.AddAsync(guest);
        }

        public async Task UpdateGuestAsync(Guest guest)
        {
            var existingGuest = await _guestRepository.GetByIdAsync(guest.Id);
            if (existingGuest == null)
            {
                throw new ArgumentException("Guest not found.");
            }

            await _guestRepository.UpdateAsync(guest);
        }

        public Task DeleteGuestAsync(int id) => _guestRepository.DeleteAsync(id);

        public Task<IEnumerable<Guest>> GetAllGuestsByWeddingIdAsync(int weddingId) =>
            _guestRepository.GetAllByConditionAsync(g => g.WeddingId == weddingId);

        public async Task<Dictionary<string, int>> GetRSVPStatsAsync(int weddingId)
        {
            var guests = await _guestRepository.GetAllByConditionAsync(g => g.WeddingId == weddingId);

            return new Dictionary<string, int>
            {
                ["Pending"] = guests.Count(g => g.RSVPStatus == "Pending"),
                ["Accepted"] = guests.Count(g => g.RSVPStatus == "Accepted"),
                ["Declined"] = guests.Count(g => g.RSVPStatus == "Declined"),
                ["Total"] = guests.Count()
            };
        }

        public async Task<int> GetTotalGuestCountAsync(int weddingId)
        {
            var guests = await _guestRepository.GetAllByConditionAsync(g => g.WeddingId == weddingId);
            return guests.Where(g => g.RSVPStatus == "Accepted")
                        .Sum(g => 1 + (g.PlusOne ?? 0));
        }
    }
}
