using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestWorld.Data.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public int ShippingAddressId { get; set; }

    [ForeignKey(nameof(ShippingAddressId))]
    public Address ShippingAddress { get; set; } = null!;

    public int? PromoCodeId { get; set; }

    [ForeignKey(nameof(PromoCodeId))]
    public PromoCode? PromoCode { get; set; }

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Pending";

    [Required]
    [Range(0.01, 1000000.00)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}