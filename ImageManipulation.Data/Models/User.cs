using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation.Data.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Username { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Password { get; set; }
}
