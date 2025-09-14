using ClamJam.Domain.Entities;

namespace CleanJam.Application.Cart;

public interface ICartRepository
{
    /// <summary>
    /// Retrieve the cart by id
    /// </summary>
    /// <param name="cartId">Cart Id</param>
    /// <returns></returns>
    Task<ShoppingCart> GetCartAsync(Guid cartId);
    /// <summary>
    /// Add an item to the shopping cart
    /// </summary>
    Task<ShoppingCart> SaveCartAsync(Guid cartId, ShoppingCart cart);

    /// <summary>
    /// Empties a cart
    /// </summary>
    /// <param name="cartId">Identifier for the cart to be emptied</param>
    /// <returns></returns>
    Task ClearCartAsync(Guid cartId);
}