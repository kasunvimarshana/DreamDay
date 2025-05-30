using DreamDay.BLL.DependencyInjection;
using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Context;
using DreamDay.DAL.DependencyInjection;
using DreamDay.DAL.Repositories.Implementations;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Get current environment (Development, Production, etc.)
var env = builder.Environment;

#region > Configuration: Database

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Entity Framework Core to use SQL Server
builder.Services.AddDbContext<DreamDayDbContext>(options =>
    options.UseSqlServer(connectionString) 
    //, ServiceLifetime.Scoped
);

#endregion

#region > Configuration: Authentication & Authorization

// Configure cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";               // Redirect to login page
        options.LogoutPath = "/auth/logout";             // Redirect after logout
        options.AccessDeniedPath = "/auth/accessdenied"; // Redirect for forbidden access
        options.Cookie.HttpOnly = true;                  // Prevent JavaScript access to the cookie
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Force HTTPS (production safe)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);       // Set expiration time
        options.SlidingExpiration = true;                        // Reset expiration on activity
    });

// Define role-based authorization policies
builder.Services.AddAuthorization(options =>
{
    // Basic Role-Based Policies
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("PlannerOnly", policy =>
        policy.RequireRole("Planner"));

    options.AddPolicy("CoupleOnly", policy =>
        policy.RequireRole("Couple"));

    options.AddPolicy("GuestOnly", policy =>
        policy.RequireRole("Guest"));

    // Combined Role Policies
    options.AddPolicy("AuthenticatedUsers", policy =>
        policy.RequireRole("Admin", "Planner", "Couple", "Guest"));

    options.AddPolicy("AdminOrPlanner", policy =>
        policy.RequireRole("Admin", "Planner"));

    options.AddPolicy("AdminOrCouple", policy =>
        policy.RequireRole("Admin", "Couple"));

    options.AddPolicy("PlannerOrCouple", policy =>
        policy.RequireRole("Planner", "Couple"));

    // Policy for Unauthenticated Users (Anonymous Access)
    options.AddPolicy("AnonymousUsers", policy =>
        policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated));

    // Policy allowing both authenticated and unauthenticated users
    options.AddPolicy("PublicAccess", policy =>
        policy.RequireAssertion(context => true)); // Always allow access

    // Policy for specific public pages (e.g., Home, About, Contact)
    options.AddPolicy("PublicPages", policy =>
        policy.RequireAssertion(context =>
            !context.User.Identity.IsAuthenticated ||
            context.User.Identity.IsAuthenticated));
});

#endregion

#region > Dependency Injection: Repositories

// Register repository interfaces and their implementations
builder.Services.AddRepositoryServices();

#endregion

#region > Dependency Injection: Services

// Register business logic and helper services
builder.Services.AddBusinessServices();

// Provide access to HttpContext in services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configure LocalStorageProviderService with web root path
builder.Services.AddSingleton<IStorageProviderService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    //string uploadsPath = Path.Combine(env.WebRootPath, "uploads");
    string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    return new LocalStorageProviderService(uploadsPath);
});

#endregion

#region > MVC Configuration

// Enable support for MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

#region > Middleware Pipeline Configuration

// Use custom error handler in non-development environments
if (!env.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Serve static files from wwwroot/
app.UseStaticFiles();

// Enable routing for controllers
app.UseRouting();

// Activate authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

#endregion

app.MapDefaultControllerRoute();

#region > Endpoint Routing

// Set up default route pattern: HomeController -> Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

// Seed default admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedDefaultAdminUser(services);
}

// Start the application
app.Run();

//


async Task SeedDefaultAdminUser(IServiceProvider services)
{
    var userService = services.GetRequiredService<IUserService>();

    // Check if admin user exists
    var adminUser = await userService.GetUserByEmailAsync("admin@dreamday.com");
    if (adminUser == null)
    {
        adminUser = new User
        {
            FullName = "Admin User",
            Email = "admin@dreamday.com",
            Password = "password",
            Role = "Admin"
        };

        await userService.CreateUserAsync(adminUser);
    }
}