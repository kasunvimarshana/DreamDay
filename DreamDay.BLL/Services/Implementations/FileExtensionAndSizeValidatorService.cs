using DreamDay.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class FileExtensionAndSizeValidatorService : IFileValidatorService
    {
        private readonly HashSet<string> _allowedExtensions;
        private readonly long _maxFileSizeInBytes;

        public FileExtensionAndSizeValidatorService(IEnumerable<string> allowedExtensions, long maxFileSizeInBytes)
        {
            _allowedExtensions = new HashSet<string>(allowedExtensions, StringComparer.OrdinalIgnoreCase);
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        public bool IsValid(string fileName, Stream fileStream)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileStream == null)
            {
                return false;
            }

            string? extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            if (extension == null)
            {
                return false;
            }

            return _allowedExtensions.Contains(extension) && fileStream.Length <= _maxFileSizeInBytes;
        }
    }
}
