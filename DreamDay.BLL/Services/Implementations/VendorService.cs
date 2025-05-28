using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class VendorService: IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public Task<IEnumerable<Vendor>> GetAllVendorsAsync() => _vendorRepository.GetAllAsync();

        public Task<Vendor?> GetVendorByIdAsync(int id) => _vendorRepository.GetByIdAsync(id);

        public Task CreateVendorAsync(Vendor vendor)
        {
            return _vendorRepository.AddAsync(vendor);
        }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            var existingVendor = await _vendorRepository.GetByIdAsync(vendor.Id);
            if (existingVendor == null)
            {
                throw new ArgumentException("Vendor not found.");
            }

            await _vendorRepository.UpdateAsync(vendor);
        }

        public Task DeleteVendorAsync(int id) => _vendorRepository.DeleteAsync(id);
    }
}
