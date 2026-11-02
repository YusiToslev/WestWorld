using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WestWorld.Data.Models;

public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = null!;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(60)]
    public string Name { get; set; } = null!;

    [MaxLength(300)]
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }

    [Required, MaxLength(500)]
    public string ImageUrl { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}

public class Size
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string Name { get; set; } = null!;

    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}

public class Color
{
    public int Id { get; set; }

    [Required, MaxLength(40)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(7)]
    public string HexCode { get; set; } = "#000000";

    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}

public class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int SizeId { get; set; }
    public Size Size { get; set; } = null!;

    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;

    [Required, MaxLength(50)]
    public string SKU { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class Favorite
{
    [Required]
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;
}

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    [Range(1, 100)]
    public int Quantity { get; set; }
}

public class Address
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    [Required, MaxLength(60)]
    public string Country { get; set; } = "България";

    [Required, MaxLength(80)]
    public string City { get; set; } = null!;

    [Required, MaxLength(200)]
    public string StreetAddress { get; set; } = null!;

    [Required, MaxLength(20)]
    public string PostalCode { get; set; } = null!;

    [Required, MaxLength(20)]
    public string PhoneNumber { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class PromoCode
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Code { get; set; } = null!;

    [Column(TypeName = "decimal(5,2)")]
    [Range(1, 100)]
    public decimal DiscountPercentage { get; set; }

    public DateTime ValidUntil { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public enum OrderStatus
{
    Pending = 0,
    Processing = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}

public class Order
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public int? PromoCodeId { get; set; }
    public PromoCode? PromoCode { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
}