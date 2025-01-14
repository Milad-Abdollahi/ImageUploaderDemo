using ImageManipulation.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation.Data.Repositories;

public interface IProductRepository
{
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> FindProductsByIdAsync(int id);
    Task DeleteProductAsync(Product product);
    Task<IEnumerable<Product>> GetProductsByUserIdAsync(int userId);
}
