using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Implementations;
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
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;

        public VenueService(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public Task<IEnumerable<Venue>> GetAllVenuesAsync() => _venueRepository.GetAllAsync();

        public Task<Venue?> GetVenueByIdAsync(int id) => _venueRepository.GetByIdAsync(id);

        public Task CreateVenueAsync(Venue venue)
        {
            return _venueRepository.AddAsync(venue);
        }

        public async Task UpdateVenueAsync(Venue venue)
        {
            var existingVenue = await _venueRepository.GetByIdAsync(venue.Id);
            if (existingVenue == null)
            {
                throw new ArgumentException("Venue not found.");
            }

            await _venueRepository.UpdateAsync(venue);
        }

        public Task DeleteVenueAsync(int id) => _venueRepository.DeleteAsync(id);
    }
}
