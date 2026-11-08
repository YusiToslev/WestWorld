using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WestWorld.Data.Entities;

namespace WestWorld.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WestWorldDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        // 1. Seed Roles
        string[] roles = ["Administrator", "Employee", "Client"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seed Demo Accounts
        await SeedUserAsync(userManager, "admin@westworld.bg", "Admin123!", "Emma", "Watson", "Administrator");
        await SeedUserAsync(userManager, "employee@westworld.bg", "Employee123!", "Doc", "Doom", "Employee");
        await SeedUserAsync(userManager, "client@westworld.bg", "Client123!", "Arthur", "Morgan", "Client");

        // 3. Seed Initial Catalog Data
        if (!await context.Categories.AnyAsync())
        {
            var hatsCategory = new Category { Name = "Cowboy Hats", Description = "Authentic felt and straw western hats." };
            var bootsCategory = new Category { Name = "Western Boots", Description = "Handcrafted leather cowboy boots." };
            var jacketsCategory = new Category { Name = "Western Jackets", Description = "Denim, suede, and fringe leather jackets." };
            context.Categories.AddRange(hatsCategory, bootsCategory, jacketsCategory);

            var sizeM = new Size { Name = "M" };
            var sizeL = new Size { Name = "L" };
            var size42 = new Size { Name = "42" };
            context.Sizes.AddRange(sizeM, sizeL, size42);

            var colorBrown = new Color { Name = "Chestnut Brown", HexCode = "#8B4513" };
            var colorBlack = new Color { Name = "Midnight Black", HexCode = "#1C1C1C" };
            context.Colors.AddRange(colorBrown, colorBlack);

            var product = new Product
            {
                Name = "Stetson Skyline 6X Cowboy Hat",
                Description = "Classic cattleman crease fur felt western hat.",
                BasePrice = 289.99m,
                ImageUrl = "/images/products/stetson-skyline.jpg",
                Category = hatsCategory
            };
            context.Products.Add(product);

            context.ProductVariants.Add(new ProductVariant
            {
                Product = product,
                Size = sizeL,
                Color = colorBrown,
                StockQuantity = 15,
                Sku = "WW-HAT-001-L-BRN"
            });

            context.PromoCodes.Add(new PromoCode
            {
                Code = "WEST10",
                DiscountPercentage = 10m,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                IsActive = true
            });

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string firstName,
        string lastName,
        string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}