using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Discount;


namespace ClamJam.Tests;

public class DiscountServiceTests
{
    [Fact]
    public void ApplyCartDiscount_WithCoupon_DiscountsItems()
    {
        var product = new Product("p", new Money(100, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        var cart = TestHelpers.CreateCart(EnumCurrency.USD, item);
        var coupon = new PercentageCoupon(new Percentage(10));

        var service = new DiscountService();
        service.ApplyCartDiscount(cart, coupon);

        Assert.Equal(90m, item.DiscountedLineTotal!.Value.Amount);
    }

    [Fact]
    public void ApplyCartDiscount_NoCoupon_KeepsLineTotal()
    {
        var product = new Product("p", new Money(50, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        var cart = TestHelpers.CreateCart(EnumCurrency.USD, item);

        var service = new DiscountService();
        service.ApplyCartDiscount(cart, null);

        Assert.Equal(item.LineTotal.Amount, item.DiscountedLineTotal!.Value.Amount);
    }
}

