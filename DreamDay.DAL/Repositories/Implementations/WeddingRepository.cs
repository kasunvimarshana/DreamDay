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
    public class WeddingRepository : IWeddingRepository
    {
        private readonly DreamDayDbContext _context;

        public WeddingRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wedding>> GetAllAsync()
        {
            return await _context.Weddings
                //.AsNoTracking()
                .Include(w => w.Venue)
                .Include(w => w.Owner)
                .ToListAsync();
        }

        public async Task<Wedding> GetByIdAsync(int id)
        {
            return await _context.Weddings
                //.AsNoTracking()
                .Include(w => w.Venue)
                .Include(w => w.Owner)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Wedding wedding)
        {
            await _context.Weddings.AddAsync(wedding);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wedding wedding)
        {
            _context.Weddings.Update(wedding);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding == null)
            {
                return;
            }
            _context.Weddings.Remove(wedding);
            await _context.SaveChangesAsync();
        }

        public async Task<Wedding?> GetByConditionAsync(Expression<Func<Wedding, bool>> predicate)
        {
            return await _context.Weddings.FirstOrDefaultAsync(predicate);
        }
    }
}
