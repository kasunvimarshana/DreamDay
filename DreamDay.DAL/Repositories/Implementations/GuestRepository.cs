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
    public class GuestRepository : IGuestRepository
    {
        private readonly DreamDayDbContext _context;

        public GuestRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guest>> GetAllAsync()
        {
            return await _context.Guests
                //.AsNoTracking()
                .Include(g => g.Wedding)
                .OrderBy(g => g.Id)
                .ThenBy(g => g.FullName)
                .ToListAsync();
        }

        public async Task<Guest?> GetByIdAsync(int id)
        {
            return await _context.Guests
                //.AsNoTracking()
                .Include(g => g.Wedding)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Guest guest)
        {
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guest guest)
        {
            _context.Guests.Update(guest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return;
            }
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
        }

        public async Task<Guest?> GetByConditionAsync(Expression<Func<Guest, bool>> predicate)
        {
            return await _context.Guests.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Guest>> GetAllByConditionAsync(Expression<Func<Guest, bool>> predicate)
        {
            return await _context.Guests
                .Where(predicate)
                .OrderBy(g => g.Id)
                .ThenBy(g => g.FullName)
                .ToListAsync();
        }
    }
}
