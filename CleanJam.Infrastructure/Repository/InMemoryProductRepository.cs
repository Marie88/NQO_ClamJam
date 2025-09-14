using ClamJam.Domain.Entities;
using CleanJam.Application.Product;
using System.Collections.Concurrent;

namespace CleanJam.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Product? GetProductAsync(Guid productId)
    {
        if (productId  == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(productId));

        return _products.TryGetValue(productId, out var product) ? product : null;
    }

    public Task<Product> SaveOrUpdateProductAsync(Product product)
    {
        if (string.IsNullOrEmpty(product.Name))
            throw new ArgumentException("Product Name cannot be null or empty");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative");

        if (product.Id == Guid.Empty)
        {
            product.Id = Guid.NewGuid();
        }
        
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task DeleteProductAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentNullException("Product ID cannot be empty");
        }

        if (!_products.TryRemove(productId, out var removedValue))
        {
            throw new Exception("Could not remove the product");
        }
        
        return Task.CompletedTask;
    }

    public IList<Product> GetProductsAsync()
    {
        return _products.Values.ToList();
    }
}