using DreamDay.BLL.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class FileHandlerService : IFileHandlerService
    {
        private readonly IStorageProviderService _storageProvider;
        private readonly IFileValidatorService _validator;
        private readonly IFileNamerService _fileNamer;

        public FileHandlerService(IStorageProviderService storageProvider, IFileValidatorService validator, IFileNamerService fileNamer)
        {
            _storageProvider = storageProvider;
            _validator = validator;
            _fileNamer = fileNamer;
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName, CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }
            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException("Invalid file name", nameof(originalFileName));
            }
            if (!_validator.IsValid(originalFileName, fileStream))
            {
                throw new InvalidOperationException("File validation failed.");
            }

            string fileName = _fileNamer.GenerateFileName(originalFileName);
            return await _storageProvider.SaveAsync(fileStream, fileName, cancellationToken);
        }

        public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }
            return await _storageProvider.DeleteAsync(filePath, cancellationToken);
        }

        public async Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }
            return await _storageProvider.GetAsync(filePath, cancellationToken);
        }

        public string GetFileUrl(string relativePath)
        {
            return $"/{relativePath.Replace("\\", "/")}";
        }
    }

}
