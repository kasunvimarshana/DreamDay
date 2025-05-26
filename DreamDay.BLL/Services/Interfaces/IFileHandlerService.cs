using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IFileHandlerService
    {
        Task<string> SaveFileAsync(Stream fileStream, string originalFileName, CancellationToken cancellationToken = default);
        Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
        Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}
