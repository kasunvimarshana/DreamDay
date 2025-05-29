using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IWeddingService
    {
        Task<IEnumerable<Wedding>> GetAllWeddingsAsync();
        Task<Wedding?> GetWeddingByIdAsync(int id);
        Task CreateWeddingAsync(Wedding wedding);
        Task UpdateWeddingAsync(Wedding wedding);
        Task DeleteWeddingAsync(int id);
    }
}
