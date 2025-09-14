using ClamJam.Domain.Dtos;
using CleanJam.Application.Cart;
using CleanJam.Application.Export;
using Microsoft.AspNetCore.Mvc;

namespace ClamJam.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IExportServiceFactory _exportServiceFactory;

    public CartController(ICartService cartService, IExportServiceFactory exportServiceFactory)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _exportServiceFactory = exportServiceFactory ?? throw new ArgumentNullException(nameof(exportServiceFactory));
    }

    /// <summary>
    ///  Add item to cart
    /// </summary>
    /// <param name="request">Item to be added into cart</param>
    [HttpPost("item")]
    public async Task<ActionResult> AddItem([FromBody] AddItemRequest request)
    {
        try
        {
            await _cartService.AddItemAsync(request);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred while adding the item to cart: {e.Message}");
        }
    }

    /// <summary>
    /// Generates the sum to be paid for a cart, including taxes and discounts 
    /// </summary>
    /// <param name="request">Cart and coupon for which the total to be paid is computed</param>
    /// <returns>The total to be paid and its currency</returns>
    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutResult>> Checkout([FromBody] CheckoutRequest request)
    {
        try
        {
            var result = await _cartService.CheckoutAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred while checking out: {e.Message}");
        }
    }

    /// <summary>
    /// A summary of the cart id
    /// </summary>
    /// <param name="id">Current cart id</param>
    /// <returns>Summary of the cart id</returns>
    [HttpGet("{id}/summary")]
    public async Task<ActionResult<CartSummary>> GetSummary(Guid id)
    {
        try
        {
            var result = await _cartService.GetCartSummaryAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred while retrieving cart summary: {e.Message}");
        }
    }

    /// <summary>
    /// Clear the current cart id
    /// </summary>
    /// <param name="id">Cart id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Clear(Guid id)
    {
        try
        {
            await _cartService.ClearCartAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred while clearing the cart: {e.Message}");
        }
    }
}