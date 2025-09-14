using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace ClamJam.Domain.Dtos;

public record AddItemRequest(Guid CartId, Guid ProductId);

public record CheckoutRequest(Guid CartId, EnumCouponCode? CouponCode, decimal? Percentage);

public record CartSummary(
    int TotalItems,
    decimal Subtotal,
    EnumCurrency Currency,
    List<CartItemDto> Items);

public record CartItemDto(string Name, decimal Price, int Quantity, decimal LineTotal);

public record CheckoutResult(decimal Total, string Currency);