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
    public class WeddingService : IWeddingService
    {
        private readonly IWeddingRepository _weddingRepository;

        public WeddingService(IWeddingRepository weddingRepository)
        {
            _weddingRepository = weddingRepository;
        }

        public Task<IEnumerable<Wedding>> GetAllWeddingsAsync() => _weddingRepository.GetAllAsync();

        public Task<Wedding?> GetWeddingByIdAsync(int id) => _weddingRepository.GetByIdAsync(id);

        public Task CreateWeddingAsync(Wedding wedding)
        {
            return _weddingRepository.AddAsync(wedding);
        }

        public async Task UpdateWeddingAsync(Wedding wedding)
        {
            var existingWedding = await _weddingRepository.GetByIdAsync(wedding.Id);
            if (existingWedding == null)
            {
                throw new ArgumentException("Wedding not found.");
            }

            await _weddingRepository.UpdateAsync(wedding);
        }

        public Task DeleteWeddingAsync(int id) => _weddingRepository.DeleteAsync(id);
    }
}
