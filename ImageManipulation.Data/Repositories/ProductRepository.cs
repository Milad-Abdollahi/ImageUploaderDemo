using ImageManipulation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageManipulation.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    // Create
    
    public async Task<Product> AddProductAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
    public async Task<Product> UpdateProductAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(Product product)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task<Product?> FindProductsByIdAsync(int id)
    {
        Product? product = await context.Products.FindAsync(id);
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        List<Product> products = await context.Products.ToListAsync();
        return products;
    }
}
