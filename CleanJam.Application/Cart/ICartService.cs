using ClamJam.Domain.Dtos;
using ClamJam.Domain.Entities;

namespace CleanJam.Application.Cart;

public interface ICartService
{
    /// <summary>
    /// Add an item to the shopping cart
    /// </summary>
    Task AddItemAsync(AddItemRequest request);
    /// <summary>
    /// Compute the total of the current shopping cart, including taxes and discounts 
    /// </summary>
    Task<CheckoutResult> CheckoutAsync(CheckoutRequest request);
    /// <summary>
    /// Retrieve a summary of the cart
    /// </summary>
    /// <param name="cartId">Cart Id</param>
    /// <returns></returns>
    Task<CartSummary> GetCartSummaryAsync(Guid cartId);
    /// <summary>
    /// Retrieve the cart by id
    /// </summary>
    /// <param name="cartId">Cart Id</param>
    /// <returns></returns>
    Task<ShoppingCart> GetCartById(Guid cartId);
    /// <summary>
    /// Clear cart of all the cart items
    /// </summary>
    Task ClearCartAsync(Guid cartId);
}