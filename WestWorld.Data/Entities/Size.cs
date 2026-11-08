using System.ComponentModel.DataAnnotations;

namespace WestWorld.Data.Entities;

public class Size
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;
}