using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestWorld.Data.Entities;

public class PromoCode
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Range(1, 90)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercentage { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }

    public bool IsActive { get; set; } = true;
}