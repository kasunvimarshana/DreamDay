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
            string extension = Path.GetExtension(fileName);
            return _allowedExtensions.Contains(extension) && fileStream.Length <= _maxFileSizeInBytes;
        }
    }
}
