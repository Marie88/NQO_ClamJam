using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Dtos;
using ClamJam.Domain.Entities;
using CleanJam.Application.Pricing;
using CleanJam.Application.Product;

namespace CleanJam.Application.Cart;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPricingService _pricingService;

    public CartService(ICartRepository cartRepository, IPricingService pricingService, IProductRepository productRepository)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task AddItemAsync(AddItemRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.CartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(request.CartId));

        if (request.ProductId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be null or empty");

        var product = _productRepository.GetProductAsync(request.ProductId);
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        var cart = await _cartRepository.GetCartAsync(request.CartId);
        var money = new Money(product.Price, cart.Currency);
        cart.AddItem(product);
        await _cartRepository.SaveCartAsync(request.CartId, cart);
    }

    public async Task<CheckoutResult> CheckoutAsync(CheckoutRequest request)
    {
        if (request.CartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(request.CartId));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var cart = await _cartRepository.GetCartAsync(request.CartId);
        var coupon = CouponFactory.Create(request.CouponCode, request.Percentage);
        var total = _pricingService.CalculateTotal(cart, coupon);

        return new CheckoutResult(total.Amount, total.Currency.ToCode());
    }

    public async Task<CartSummary> GetCartSummaryAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));

        var cart = await _cartRepository.GetCartAsync(cartId);
        var items = cart.Items.Select(item => new CartItemDto(
            item.Product.Name,
            item.Product.Price.Amount,
            item.Quantity,
            item.LineTotal.Amount)).ToList();

        return new CartSummary(
            cart.TotalItemCount,
            cart.Subtotal.Amount,
            cart.Currency,
            items);
    }

    public async Task ClearCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));

        await _cartRepository.ClearCartAsync(cartId);
    }

    public async Task<ShoppingCart> GetCartById(Guid cartId)
    {
        return await _cartRepository.GetCartAsync(cartId);
    }
}