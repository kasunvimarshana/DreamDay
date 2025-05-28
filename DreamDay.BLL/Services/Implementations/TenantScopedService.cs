using DreamDay.DAL.Context;
using DreamDay.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    //
    public class TenantScopedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DreamDayDbContext _context;

        public TenantScopedService(DreamDayDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _httpContextAccessor = accessor;
        }

        /// <summary>
        /// Attempts to get the TenantId for the current user. Returns null if not assigned.
        /// </summary>
        public int? GetTenantId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return null;
            }

            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId);

            return user?.TenantId; // nullable int
        }

        /// <summary>
        /// Returns users scoped by tenant if TenantId exists, otherwise returns all users.
        /// </summary>
        public IQueryable<User> GetUsersForTenant()
        {
            var tenantId = GetTenantId();

            if (tenantId.HasValue)
            {
                return _context.Users.Where(u => u.TenantId == tenantId.Value);
            }
            else
            {
                // TenantId not assigned: 
                // Option 1: Return empty list to restrict access
                // return Enumerable.Empty<User>().AsQueryable();

                // Option 2: Return all users (less secure)
                return _context.Users;
            }
        }
    }
}
