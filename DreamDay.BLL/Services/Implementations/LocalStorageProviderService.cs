using DreamDay.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class LocalStorageProviderService : IStorageProviderService
    {
        private readonly string _basePath;

        public LocalStorageProviderService(string basePath)
        {
            _basePath = basePath;
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
        {
            string fullPath = Path.Combine(_basePath, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            using var fileStreamOutput = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await fileStream.CopyToAsync(fileStreamOutput, 81920, cancellationToken);

            return fullPath;
        }

        public Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Stream> GetAsync(string filePath, CancellationToken cancellationToken)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(stream);
        }
    }
}
