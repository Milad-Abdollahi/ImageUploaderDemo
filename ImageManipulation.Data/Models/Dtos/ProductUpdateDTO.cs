using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation.Data.Models.Dtos;

public class ProductUpdateDTO
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public IFormFile? ImageFile { get; set; }
}
