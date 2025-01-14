using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ImageManipulation.Data.Models.Dtos;

public class ProductDTO
{
    [Required]
    [MaxLength(30)]
    public string? ProductName { get; set; }

    [Required]
    public IFormFile? ImageFile { get; set; }
}
