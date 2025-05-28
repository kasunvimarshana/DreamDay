using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Implementations
{
    public class VendorRepository: IVendorRepository
    {
        private readonly DreamDayDbContext _context;

        public VendorRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vendor>> GetAllAsync() => await _context.Vendors.ToListAsync();

        public async Task<Vendor?> GetByIdAsync(int id) => await _context.Vendors.FindAsync(id);

        public async Task AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return;
            }
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task<Vendor?> GetByConditionAsync(Expression<Func<Vendor, bool>> predicate)
        {
            return await _context.Vendors.FirstOrDefaultAsync(predicate);
        }
    }
}
