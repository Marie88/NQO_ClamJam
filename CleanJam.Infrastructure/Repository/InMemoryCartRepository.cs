using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Cart;
using System.Collections.Concurrent;

namespace CleanJam.Infrastructure.Repository;

public class InMemoryCartRepository : ICartRepository
{
    private readonly ConcurrentDictionary<Guid, ShoppingCart> _carts = new();
    private readonly EnumCurrency _defaultCurrency;

    public InMemoryCartRepository(EnumCurrency defaultCurrency = EnumCurrency.USD)
    {
        _defaultCurrency = defaultCurrency;
    }

    public Task<ShoppingCart> GetCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));

        var cart = _carts.GetOrAdd(cartId, _ => new ShoppingCart(_defaultCurrency));
        return Task.FromResult(cart);
    }

    public Task<ShoppingCart> SaveCartAsync(Guid cartId, ShoppingCart cart)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
        
        if (cart == null)
            throw new ArgumentNullException(nameof(cart));

        _carts[cartId] = cart;
        return Task.FromResult(cart);
    }

    public Task ClearCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));

        _carts.TryRemove(cartId, out _);
        return Task.CompletedTask;
    }
}