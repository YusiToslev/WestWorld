using System.ComponentModel.DataAnnotations;

namespace WestWorld.Data.Entities;

public class Color
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(7)]
    public string HexCode { get; set; } = string.Empty;
}