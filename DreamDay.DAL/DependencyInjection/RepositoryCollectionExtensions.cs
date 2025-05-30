﻿using DreamDay.DAL.Repositories.Implementations;
using DreamDay.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.DependencyInjection
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IWeddingRepository, WeddingRepository>();
            services.AddScoped<IWeddingChecklistItemRepository, WeddingChecklistItemRepository>();
            services.AddScoped<IBudgetItemRepository, BudgetItemRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            return services;
        }
    }

}
