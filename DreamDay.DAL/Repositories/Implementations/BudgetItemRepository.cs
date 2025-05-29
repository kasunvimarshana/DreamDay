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
    public class BudgetItemRepository : IBudgetItemRepository
    {
        private readonly DreamDayDbContext _context;

        public BudgetItemRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BudgetItem>> GetAllAsync()
        {
            return await _context.BudgetItems
                //.AsNoTracking()
                .Include(b => b.Wedding)
                .ToListAsync();
        }

        public async Task<BudgetItem?> GetByIdAsync(int id)
        {
            return await _context.BudgetItems
                //.AsNoTracking()
                .Include(b => b.Wedding)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(BudgetItem budgetItem)
        {
            await _context.BudgetItems.AddAsync(budgetItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BudgetItem budgetItem)
        {
            _context.BudgetItems.Update(budgetItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var budgetItem = await _context.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return;
            }
            _context.BudgetItems.Remove(budgetItem);
            await _context.SaveChangesAsync();
        }

        public async Task<BudgetItem?> GetByConditionAsync(Expression<Func<BudgetItem, bool>> predicate)
        {
            return await _context.BudgetItems.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<BudgetItem>> GetAllByConditionAsync(Expression<Func<BudgetItem, bool>> predicate)
        {
            return await _context.BudgetItems.Where(predicate).ToListAsync();
        }
    }
}
