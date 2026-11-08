using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestWorld.Data;
using WestWorld.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context (reads ConnectionStrings:DefaultConnection from User Secrets)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<WestWorldDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity & Roles Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<WestWorldDbContext>()
.AddDefaultTokenProviders();

// 3. Secure Session Cookie Settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Run Migrations and Seeding automatically on startup
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();