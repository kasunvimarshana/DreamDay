using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class BudgetItemService : IBudgetItemService
    {
        private readonly IBudgetItemRepository _budgetItemRepository;

        public BudgetItemService(IBudgetItemRepository budgetItemRepository)
        {
            _budgetItemRepository = budgetItemRepository;
        }

        public Task<IEnumerable<BudgetItem>> GetAllBudgetItemsAsync() => _budgetItemRepository.GetAllAsync();

        public Task<BudgetItem?> GetBudgetItemByIdAsync(int id) => _budgetItemRepository.GetByIdAsync(id);

        public Task CreateBudgetItemAsync(BudgetItem budgetItem)
        {
            return _budgetItemRepository.AddAsync(budgetItem);
        }

        public async Task UpdateBudgetItemAsync(BudgetItem budgetItem)
        {
            var existingBudgetItem = await _budgetItemRepository.GetByIdAsync(budgetItem.Id);
            if (existingBudgetItem == null)
            {
                throw new ArgumentException("Budget item not found.");
            }

            await _budgetItemRepository.UpdateAsync(budgetItem);
        }

        public Task DeleteBudgetItemAsync(int id) => _budgetItemRepository.DeleteAsync(id);

        public Task<IEnumerable<BudgetItem>> GetAllBudgetItemsByWeddingIdAsync(int weddingId) =>
            _budgetItemRepository.GetAllByConditionAsync(bi => bi.WeddingId == weddingId);
    }
}
