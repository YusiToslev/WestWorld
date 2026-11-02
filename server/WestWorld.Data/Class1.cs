using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestWorld.Data.Models;

namespace WestWorld.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Size> Sizes => Set<Size>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Съставен първичен ключ за Любими продукти
        builder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.ProductId });

        // Уникална комбинация от Продукт + Размер + Цвят
        builder.Entity<ProductVariant>()
            .HasIndex(pv => new { pv.ProductId, pv.SizeId, pv.ColorId })
            .IsUnique();

        builder.Entity<ProductVariant>()
            .HasIndex(pv => pv.SKU)
            .IsUnique();

        builder.Entity<PromoCode>()
            .HasIndex(pc => pc.Code)
            .IsUnique();

        // Ограничаване на каскадното изтриване за MS SQL Server
        builder.Entity<Order>()
            .HasOne(o => o.Address)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.ProductVariant)
            .WithMany(pv => pv.OrderItems)
            .HasForeignKey(oi => oi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}