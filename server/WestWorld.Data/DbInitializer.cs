using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WestWorld.Data.Models;

namespace WestWorld.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Автоматично прилагане на миграциите при стартиране
        await context.Database.MigrateAsync();

        // 1. Seeding на ролите
        string[] roles = { "Administrator", "Employee", "Client" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seeding на демо акаунти
        await SeedUserAsync(userManager, "admin@westworld.bg", "Admin123!", "Wyatt", "Earp", "Administrator");
        await SeedUserAsync(userManager, "employee@westworld.bg", "Employee123!", "Doc", "Holiday", "Employee");
        await SeedUserAsync(userManager, "client@westworld.bg", "Client123!", "Jesse", "James", "Client");

        // 3. Seeding на каталог, размери, цветове и промокодове на английски
        if (!await context.Categories.AnyAsync())
        {
            var bootsCategory = new Category { Name = "Cowboy Boots", Description = "Authentic leather boots for riding and casual wear." };
            var hatsCategory = new Category { Name = "Western Hats", Description = "Classic Stetson hats and genuine felt models." };
            var jacketsCategory = new Category { Name = "Leather Jackets & Vests", Description = "Western suede and genuine leather jackets with fringes." };
            await context.Categories.AddRangeAsync(bootsCategory, hatsCategory, jacketsCategory);

            var sizeM = new Size { Name = "M" };
            var sizeL = new Size { Name = "L" };
            var size42 = new Size { Name = "42" };
            var size43 = new Size { Name = "43" };
            await context.Sizes.AddRangeAsync(sizeM, sizeL, size42, size43);

            var brownColor = new Color { Name = "Dark Brown", HexCode = "#5C4033" };
            var blackColor = new Color { Name = "Black", HexCode = "#1C1C1C" };
            var sandColor = new Color { Name = "Sand Suede", HexCode = "#C2B280" };
            await context.Colors.AddRangeAsync(brownColor, blackColor, sandColor);

            var bootsProduct = new Product
            {
                Name = "Texas Ranger Leather Boots",
                Description = "Handcrafted cowboy boots made of cowhide leather with traditional embroidery.",
                BasePrice = 320.00m,
                ImageUrl = "https://images.unsplash.com/photo-1542838132-92c53300491e",
                Category = bootsCategory
            };

            var hatProduct = new Product
            {
                Name = "Durango Western Hat",
                Description = "Wool western hat featuring a leather band and a metallic buckle.",
                BasePrice = 145.00m,
                ImageUrl = "https://images.unsplash.com/photo-1514327605112-b887c0e61c0a",
                Category = hatsCategory
            };

            await context.Products.AddRangeAsync(bootsProduct, hatProduct);
            await context.SaveChangesAsync();

            var variants = new List<ProductVariant>
            {
                new() { ProductId = bootsProduct.Id, SizeId = size42.Id, ColorId = brownColor.Id, SKU = "WW-BOOT-TR-42-BRN", StockQuantity = 10 },
                new() { ProductId = bootsProduct.Id, SizeId = size43.Id, ColorId = blackColor.Id, SKU = "WW-BOOT-TR-43-BLK", StockQuantity = 6 },
                new() { ProductId = hatProduct.Id, SizeId = sizeM.Id, ColorId = sandColor.Id, SKU = "WW-HAT-DUR-M-SND", StockQuantity = 15 },
                new() { ProductId = hatProduct.Id, SizeId = sizeL.Id, ColorId = brownColor.Id, SKU = "WW-HAT-DUR-L-BRN", StockQuantity = 8 }
            };
            await context.ProductVariants.AddRangeAsync(variants);

            var promo = new PromoCode
            {
                Code = "WEST20",
                DiscountPercentage = 20.00m,
                ValidUntil = DateTime.UtcNow.AddMonths(6),
                IsActive = true
            };
            await context.PromoCodes.AddAsync(promo);

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