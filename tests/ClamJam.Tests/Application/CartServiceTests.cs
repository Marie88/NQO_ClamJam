
namespace ClamJam.Tests.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClamJam.Domain.DomainModels;
    using ClamJam.Domain.Dtos;
    using ClamJam.Domain.Entities;
    using CleanJam.Application.Cart;
    using CleanJam.Application.Pricing;
    using CleanJam.Application.Product;



    public class CartServiceTests
    {
        private class StubCartRepository : ICartRepository
        {
            public bool SaveCalled;
            public bool ClearCalled;
            public Guid LastCartId;
            public ShoppingCart Cart = new(EnumCurrency.USD);

            public Task ClearCartAsync(Guid cartId)
            {
                ClearCalled = true;
                LastCartId = cartId;
                return Task.CompletedTask;
            }

            public Task<ShoppingCart> GetCartAsync(Guid cartId)
            {
                LastCartId = cartId;
                return Task.FromResult(Cart);
            }

            public Task<ShoppingCart> SaveCartAsync(Guid cartId, ShoppingCart cart)
            {
                SaveCalled = true;
                LastCartId = cartId;
                Cart = cart;
                return Task.FromResult(cart);
            }
        }

        private class StubProductRepository : IProductRepository
        {
            public Product? Product;
            public Product? GetProductAsync(Guid productId) => Product;
            public IList<Product> GetProductsAsync() => new List<Product>();
            public Task<Product> SaveOrUpdateProductAsync(Product product) => Task.FromResult(product);
            public Task DeleteProductAsync(Guid productId) => Task.CompletedTask;
        }

        private class StubPricingService : IPricingService
        {
            public Money Result = new Money(0, EnumCurrency.USD);
            public ShoppingCart? LastCart;
            public Coupon? LastCoupon;
            public Money CalculateTotal(ShoppingCart cart, Coupon? coupon = null)
            {
                LastCart = cart;
                LastCoupon = coupon;
                return Result;
            }
        }

        [Fact]
        public async Task AddItemAsync_Valid_SavesCart()
        {
            var cartRepo = new StubCartRepository();
            var prodRepo = new StubProductRepository();
            var pricing = new StubPricingService();
            var productId = Guid.NewGuid();
            prodRepo.Product = new Product(productId, "p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);

            var service = new CartService(cartRepo, pricing, prodRepo);
            await service.AddItemAsync(new AddItemRequest(Guid.NewGuid(), productId));

            Assert.True(cartRepo.SaveCalled);
        }

        [Fact]
        public async Task AddItemAsync_ProductNotFound_Throws()
        {
            var service = new CartService(new StubCartRepository(), new StubPricingService(), new StubProductRepository());
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddItemAsync(new AddItemRequest(Guid.NewGuid(), Guid.NewGuid())));
        }

        [Fact]
        public async Task CheckoutAsync_ReturnsPricingResult()
        {
            var cartRepo = new StubCartRepository();
            var pricing = new StubPricingService { Result = new Money(42, EnumCurrency.USD) };
            var service = new CartService(cartRepo, pricing, new StubProductRepository());

            var result = await service.CheckoutAsync(new CheckoutRequest(Guid.NewGuid(), null, null));

            Assert.Equal(42m, result.Total);
            Assert.Equal("usd", result.Currency);
        }

        [Fact]
        public async Task GetCartSummaryAsync_ReturnsData()
        {
            var cart = TestHelpers.CreateCart(EnumCurrency.USD);
            var cartRepo = new StubCartRepository { Cart = cart };
            var service = new CartService(cartRepo, new StubPricingService(), new StubProductRepository());

            var summary = await service.GetCartSummaryAsync(Guid.NewGuid());

            Assert.Equal(0, summary.TotalItems);
            Assert.Equal(0m, summary.Subtotal);
        }

        [Fact]
        public async Task ClearCartAsync_CallsRepository()
        {
            var cartRepo = new StubCartRepository();
            var service = new CartService(cartRepo, new StubPricingService(), new StubProductRepository());

            await service.ClearCartAsync(Guid.NewGuid());

            Assert.True(cartRepo.ClearCalled);
        }
    }


}
