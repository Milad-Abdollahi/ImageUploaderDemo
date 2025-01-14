using ImageManipulation.Data.Models;
using ImageManipulation.Data.Models.Dtos;
using ImageManipulation.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageManipulation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductRepository productRepository, ILogger<ProductController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productToAdd)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim is null)
            {
                return Unauthorized("No user claim found in the token.");
            }

            int creatorUserId = int.Parse(userIdClaim.Value);

            if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File Size should not exees 1mb");
            }

            string[] allowedFileExtensions = [".jpg", ".png", ".jpeg"];
            string ext = Path.GetExtension(productToAdd.ImageFile.FileName).ToLowerInvariant();
            if (!allowedFileExtensions.Contains(ext))
            {
                return BadRequest($"Only {string.Join(", ", allowedFileExtensions)} files are allowed.");
            }

            byte[]? imageData;
            using (var ms = new MemoryStream())
            {
                await productToAdd.ImageFile.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            // mapping ProductDto to product Entity
            var product = new Product
            {
                ProductName = productToAdd.ProductName,
                ProductImageData = imageData,
                CreatorUserId = creatorUserId,
            };

            var createdProduct = await productRepository.AddProductAsync(product);

            return CreatedAtAction(nameof(CreateProduct), createdProduct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> GetProducts()
    {
        IEnumerable<Product> product = await productRepository.GetProductsAsync();
        return Ok(product);
    }

    [HttpGet("my-Products")]
    [Authorize]
    public async Task<IActionResult> GetMyProducts()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        if (userIdClaim is null)
        {
            return Unauthorized("No userId claim found in token.");
        }
        int currentUserId = int.Parse(userIdClaim.Value);

        // 2. Fetch products from the repository where the CreatorUserId matches
        //    your repository can have a method like GetProductsByUserIdAsync, or you can do it inline.

        var product = await productRepository.GetProductsByUserIdAsync(currentUserId);

        return Ok(product); 
    }

}
