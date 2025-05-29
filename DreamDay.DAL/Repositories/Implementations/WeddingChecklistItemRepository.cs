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
    public class WeddingChecklistItemRepository: IWeddingChecklistItemRepository
    {
        private readonly DreamDayDbContext _context;

        public WeddingChecklistItemRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeddingChecklistItem>> GetAllAsync()
        {
            return await _context.WeddingChecklistItems
                //.AsNoTracking()
                .Include(w => w.Wedding)
                .ToListAsync();
        }

        public async Task<WeddingChecklistItem> GetByIdAsync(int id)
        {
            return await _context.WeddingChecklistItems
                //.AsNoTracking()
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(WeddingChecklistItem weddingChecklistItem)
        {
            await _context.WeddingChecklistItems.AddAsync(weddingChecklistItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WeddingChecklistItem weddingChecklistItem)
        {
            _context.WeddingChecklistItems.Update(weddingChecklistItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var weddingChecklistItem = await _context.WeddingChecklistItems.FindAsync(id);
            if (weddingChecklistItem == null)
            {
                return;
            }
            _context.WeddingChecklistItems.Remove(weddingChecklistItem);
            await _context.SaveChangesAsync();
        }

        public async Task<WeddingChecklistItem?> GetByConditionAsync(Expression<Func<WeddingChecklistItem, bool>> predicate)
        {
            return await _context.WeddingChecklistItems.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<WeddingChecklistItem>> GetAllByConditionAsync(Expression<Func<WeddingChecklistItem, bool>> predicate)
        {
            return await _context.WeddingChecklistItems.Where(predicate).ToListAsync();
        }
    }
}
