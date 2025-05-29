using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IBudgetItemService
    {
        Task<IEnumerable<BudgetItem>> GetAllBudgetItemsAsync();
        Task<BudgetItem?> GetBudgetItemByIdAsync(int id);
        Task CreateBudgetItemAsync(BudgetItem budgetItem);
        Task UpdateBudgetItemAsync(BudgetItem budgetItem);
        Task DeleteBudgetItemAsync(int id);
        Task<IEnumerable<BudgetItem>> GetAllBudgetItemsByWeddingIdAsync(int weddingId);
    }
}
