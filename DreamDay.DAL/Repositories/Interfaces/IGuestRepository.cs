using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IGuestRepository
    {
        Task<IEnumerable<Guest>> GetAllAsync();
        Task<Guest?> GetByIdAsync(int id);
        Task AddAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(int id);
        Task<Guest?> GetByConditionAsync(Expression<Func<Guest, bool>> predicate);
        Task<IEnumerable<Guest>> GetAllByConditionAsync(Expression<Func<Guest, bool>> predicate);
    }
}
