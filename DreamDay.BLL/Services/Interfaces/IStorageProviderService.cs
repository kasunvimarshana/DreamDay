using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IStorageProviderService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken);
        Task<Stream> GetAsync(string filePath, CancellationToken cancellationToken);
    }
}
