using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IWeddingChecklistItemRepository
    {
        Task<IEnumerable<WeddingChecklistItem>> GetAllAsync();
        Task<WeddingChecklistItem?> GetByIdAsync(int id);
        Task AddAsync(WeddingChecklistItem weddingChecklistItem);
        Task UpdateAsync(WeddingChecklistItem weddingChecklistItem);
        Task DeleteAsync(int id);
        Task<WeddingChecklistItem?> GetByConditionAsync(Expression<Func<WeddingChecklistItem, bool>> predicate);
        Task<IEnumerable<WeddingChecklistItem>> GetAllByConditionAsync(Expression<Func<WeddingChecklistItem, bool>> predicate);
    }
}
