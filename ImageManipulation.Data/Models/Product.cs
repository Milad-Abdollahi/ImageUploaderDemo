
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
    public byte[]? ProductImageData { get; set; }

    [Required]
    public int CreatorUserId { get; set; }

    [ForeignKey(nameof(CreatorUserId))]
    public User? CreatorUser { get; set; }
}
