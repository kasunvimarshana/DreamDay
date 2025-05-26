using DreamDay.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class TimestampGuidFileNamerService : IFileNamerService
    {
        public string GenerateFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string guid = Guid.NewGuid().ToString("N");
            return $"{timestamp}_{guid}{extension}";
        }
    }
}
