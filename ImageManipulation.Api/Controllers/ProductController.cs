using ImageManipulation.Data.Models;
using ImageManipulation.Data.Models.Dtos;
using ImageManipulation.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageManipulation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IFileService fileService, IProductRepository productRepository, ILogger<ProductController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productToAdd)
    {
        try
        {
            if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File Size should not exees 1mb");
            }
            string[] allowedFileExtentions = [".jpg", ".png", ".jpeg"];
            string createdImageName = await fileService.SaveFileAsync(productToAdd.ImageFile, allowedFileExtentions);

            // mapping ProductDto to product manually
            var product = new Product
            {
                ProductName = productToAdd.ProductName,
                ProductImage = createdImageName,
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
    public async Task<IActionResult> GetProducts()
    {
        var product = await productRepository.GetProductsAsync();
        return Ok(product);
    }
}
