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
        private readonly string _physicalBasePath;
        private readonly string _webBasePath;

        public LocalStorageProviderService(string physicalBasePath, string webBasePath = "uploads")
        {
            _physicalBasePath = physicalBasePath;
            _webBasePath = webBasePath;
            //Directory.CreateDirectory(_physicalBasePath); // Ensure directory exists
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
        {
            string fullPhysicalPath = Path.Combine(_physicalBasePath, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPhysicalPath)!);

            using var fileStreamOutput = new FileStream(fullPhysicalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await fileStream.CopyToAsync(fileStreamOutput, 81920, cancellationToken);

            // Return web-relative path instead of full physical path
            return Path.Combine(_webBasePath, fileName).Replace('\\', '/');
        }

        public Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken)
        {
            // Convert web path back to physical path for deletion
            string physicalPath = ConvertWebPathToPhysicalPath(filePath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Stream> GetAsync(string filePath, CancellationToken cancellationToken)
        {
            // Convert web path back to physical path for reading
            string physicalPath = ConvertWebPathToPhysicalPath(filePath);

            if (!File.Exists(physicalPath))
            {
                throw new FileNotFoundException("File not found", physicalPath);
            }

            Stream stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(stream);
        }

        private string ConvertWebPathToPhysicalPath(string webPath)
        {
            // Handle both forward and back slashes, and remove leading slash if present
            string normalizedPath = webPath.Replace('/', Path.DirectorySeparatorChar);
            if (normalizedPath.StartsWith(_webBasePath))
            {
                // Extract just the filename part
                string fileName = normalizedPath.Substring(_webBasePath.Length).TrimStart(Path.DirectorySeparatorChar);
                return Path.Combine(_physicalBasePath, fileName);
            }

            // Fallback: assume it's already a physical path or just a filename
            return Path.IsPathRooted(webPath) ? webPath : Path.Combine(_physicalBasePath, webPath);
        }
    }
}
