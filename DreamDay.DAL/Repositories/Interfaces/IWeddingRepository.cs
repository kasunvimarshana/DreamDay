using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IWeddingRepository
    {
        Task<IEnumerable<Wedding>> GetAllAsync();
        Task<Wedding?> GetByIdAsync(int id);
        Task AddAsync(Wedding wedding);
        Task UpdateAsync(Wedding wedding);
        Task DeleteAsync(int id);
        Task<Wedding?> GetByConditionAsync(Expression<Func<Wedding, bool>> predicate);
    }
}
