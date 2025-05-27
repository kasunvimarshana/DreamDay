using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Implementations
{
    public class VenueRepository : IVenueRepository
    {
        private readonly DreamDayDbContext _context;

        public VenueRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venue>> GetAllAsync() => await _context.Venues.ToListAsync();

        public async Task<Venue?> GetByIdAsync(int id) => await _context.Venues.FindAsync(id);

        public async Task AddAsync(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return;
            }
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
        }

        public async Task<Venue?> GetByConditionAsync(Expression<Func<Venue, bool>> predicate)
        {
            return await _context.Venues.FirstOrDefaultAsync(predicate);
        }
    }
}
