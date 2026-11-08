using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestWorld.Data.Entities;

public class ProductVariant
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [Required]
    public int SizeId { get; set; }

    [ForeignKey(nameof(SizeId))]
    public Size Size { get; set; } = null!;

    [Required]
    public int ColorId { get; set; }

    [ForeignKey(nameof(ColorId))]
    public Color Color { get; set; } = null!;

    [Required]
    [Range(0, 10000)]
    public int StockQuantity { get; set; }

    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;
}