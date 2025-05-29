using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IBudgetItemRepository
    {
        Task<IEnumerable<BudgetItem>> GetAllAsync();
        Task<BudgetItem?> GetByIdAsync(int id);
        Task AddAsync(BudgetItem budgetItem);
        Task UpdateAsync(BudgetItem budgetItem);
        Task DeleteAsync(int id);
        Task<BudgetItem?> GetByConditionAsync(Expression<Func<BudgetItem, bool>> predicate);
        Task<IEnumerable<BudgetItem>> GetAllByConditionAsync(Expression<Func<BudgetItem, bool>> predicate);
    }
}
