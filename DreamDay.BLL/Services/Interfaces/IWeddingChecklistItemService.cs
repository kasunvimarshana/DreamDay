using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IWeddingChecklistItemService
    {
        Task<IEnumerable<WeddingChecklistItem>> GetAllWeddingChecklistItemsAsync();
        Task<WeddingChecklistItem?> GetWeddingChecklistItemByIdAsync(int id);
        Task CreateWeddingChecklistItemAsync(WeddingChecklistItem weddingChecklistItem);
        Task UpdateWeddingChecklistItemAsync(WeddingChecklistItem weddingChecklistItem);
        Task DeleteWeddingChecklistItemAsync(int id);
        Task<IEnumerable<WeddingChecklistItem>> GetAllWeddingChecklistItemsByWeddingIdAsync(int weddingId);
    }
}
