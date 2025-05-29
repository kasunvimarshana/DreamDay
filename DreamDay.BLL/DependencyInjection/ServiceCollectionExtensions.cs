using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DreamDay.BLL.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string[] AllowedFileExtensions = new[] { ".jpg", ".png" };
        private const int MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IWeddingService, WeddingService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileHandlerService, FileHandlerService>();
            services.AddScoped<IFileValidatorService>(_ =>
                new FileExtensionAndSizeValidatorService(AllowedFileExtensions, MaxFileSizeInBytes));
            services.AddScoped<IFileNamerService, TimestampGuidFileNamerService>();

            return services;
        }
    }

}
