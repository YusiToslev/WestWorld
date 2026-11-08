using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestWorld.Data.Entities;

namespace WestWorld.Data;

public class WestWorldDbContext : IdentityDbContext<ApplicationUser>
{
    public WestWorldDbContext(DbContextOptions<WestWorldDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Size> Sizes => Set<Size>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>()
            .HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.ProductVariant)
            .WithMany()
            .HasForeignKey(oi => oi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraints
        builder.Entity<ProductVariant>()
            .HasIndex(pv => pv.Sku)
            .IsUnique();

        builder.Entity<PromoCode>()
            .HasIndex(pc => pc.Code)
            .IsUnique();
    }
}