using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Implementations;
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
    public class WeddingChecklistItemService :  IWeddingChecklistItemService
    {
        private readonly IWeddingChecklistItemRepository _weddingChecklistItemRepository;

        public WeddingChecklistItemService(IWeddingChecklistItemRepository weddingChecklistItemRepository)
        {
            _weddingChecklistItemRepository = weddingChecklistItemRepository;
        }

        public Task<IEnumerable<WeddingChecklistItem>> GetAllWeddingChecklistItemsAsync() => _weddingChecklistItemRepository.GetAllAsync();

        public Task<WeddingChecklistItem?> GetWeddingChecklistItemByIdAsync(int id) => _weddingChecklistItemRepository.GetByIdAsync(id);

        public Task CreateWeddingChecklistItemAsync(WeddingChecklistItem weddingChecklistItem)
        {
            return _weddingChecklistItemRepository.AddAsync(weddingChecklistItem);
        }

        public async Task UpdateWeddingChecklistItemAsync(WeddingChecklistItem weddingChecklistItem)
        {
            var existingWeddingChecklistItem = await _weddingChecklistItemRepository.GetByIdAsync(weddingChecklistItem.Id);
            if (existingWeddingChecklistItem == null)
            {
                throw new ArgumentException("Wedding not found.");
            }

            await _weddingChecklistItemRepository.UpdateAsync(weddingChecklistItem);
        }

        public Task DeleteWeddingChecklistItemAsync(int id) => _weddingChecklistItemRepository.DeleteAsync(id);

        public Task<IEnumerable<WeddingChecklistItem>> GetAllWeddingChecklistItemsByWeddingIdAsync(int weddingId) => _weddingChecklistItemRepository.GetAllByConditionAsync(wci => wci.WeddingId == weddingId);
    }
}
