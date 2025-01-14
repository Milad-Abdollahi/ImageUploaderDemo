
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageManipulation.Data.Models;

[Table("Product")]
public class Product
{
    public int Id { get; set; }
    [Required]
    [MaxLength(300)]
    public string? ProductName { get; set; }



    [Required]
    [MaxLength(50)]
    public string? ProductImage { get; set; }
}
