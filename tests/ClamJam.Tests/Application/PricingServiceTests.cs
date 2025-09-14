using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Discount;
using CleanJam.Application.Pricing;
using CleanJam.Application.Tax;

namespace ClamJam.Tests;

public class PricingServiceTests
{
    private class StubTaxService : ITaxService
    {
        public decimal TaxToReturn;
        public decimal ComputeTax(IReadOnlyCollection<CartItem> cartItems) => TaxToReturn;
        public ITaxStrategy GetVatTypeForItem(CartItem cartItem) => throw new NotImplementedException();
    }

    private class StubDiscountService : IDiscountService
    {
        public bool Called;
        public void ApplyCartDiscount(ShoppingCart cart, Coupon? coupon)
        {
            Called = true;
            foreach (var item in cart.Items)
            {
                item.DiscountedLineTotal = new Money(80, cart.Currency);
            }
        }
    }

    [Fact]
    public void CalculateTotal_EmptyCart_ReturnsZero()
    {
        var service = new PricingService(new StubTaxService(), new StubDiscountService());
        var cart = TestHelpers.CreateCart(EnumCurrency.USD);

        var total = service.CalculateTotal(cart);

        Assert.Equal(0m, total.Amount);
    }

    [Fact]
    public void CalculateTotal_WithCoupon_AppliesDiscountAndTax()
    {
        var tax = new StubTaxService { TaxToReturn = 5 };
        var discount = new StubDiscountService();
        var service = new PricingService(tax, discount);

        var product = new Product("p", new Money(100, EnumCurrency.USD), "d", EnumProductType.Food);
        var cart = TestHelpers.CreateCart(EnumCurrency.USD, new CartItem(product, 1));

        var total = service.CalculateTotal(cart, new FreeCoupon());

        Assert.True(discount.Called);
        Assert.Equal(85m, total.Amount);
    }
}

