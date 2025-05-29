using DreamDay.BLL.DependencyInjection;
using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Context;
using DreamDay.DAL.DependencyInjection;
using DreamDay.DAL.Repositories.Implementations;
using DreamDay.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Get current environment (Development, Production, etc.)
var env = builder.Environment;

#region > Configuration: Database

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Entity Framework Core to use SQL Server
builder.Services.AddDbContext<DreamDayDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

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
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));                  // Only Admins
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));        // Admin or User
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

// Start the application
app.Run();
