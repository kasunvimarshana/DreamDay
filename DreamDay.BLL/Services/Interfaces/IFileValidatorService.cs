using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IFileValidatorService
    {
        bool IsValid(string fileName, Stream fileStream);
    }
}
