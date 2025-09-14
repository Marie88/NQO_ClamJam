using ClamJam.Domain.Dtos;
using ClamJam.Domain.Entities;
using CleanJam.Application.Product;
using Microsoft.AspNetCore.Mvc;

namespace ClamJam.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
   

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>A list of products</returns>
    [HttpGet]
    public ActionResult<IList<Product>> GetProducts()
    {
        try
        {
            var result = _productRepository.GetProductsAsync();
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding the item to cart");
        }
    }

    /// <summary>
    /// Retrieve a product by its id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Product</returns>
    [HttpGet("{id}")]
    public ActionResult<IList<Product>> GetProduct(Guid id)
    {
        try
        {
            var result = _productRepository.GetProductAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding the item to cart");
        }
    }

    /// <summary>
    /// Add a new product to the catalogue
    /// </summary>
    /// <param name="request">Product</param>
    /// <returns>Newly created product</returns>
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct([FromBody] AddProductRequest request)
    {
        try
        {
            var product = new Product(request.Name, request.Price, request.Description, request.ProductType);
            var result = await _productRepository.SaveOrUpdateProductAsync(product);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding the item to cart");
        }
    }

    /// <summary>
    /// Update the product
    /// </summary>
    /// <param name="request">Update product request</param>
    /// <returns>Updated product</returns>
    [HttpPut]
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] UpdateProductRequest request)
    {
        try
        {
            var product = new Product(request.Id,request.Name, request.Price, request.Description, request.ProductType);
            var result = await _productRepository.SaveOrUpdateProductAsync(product);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding the item to cart");
        }
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="id">Id of the product</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        try
        {
            await _productRepository.DeleteProductAsync(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred during checkout");
        }
    }
}