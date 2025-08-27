using ClamJam.Library;

namespace ClamJam.Tests;

public class ShopTests
{
    [Fact]
    public void AddsItems_UsingWeirdFormat()
    {
        var shop = new ShopManager();
        var result = shop.Add("Apple", 1.23);
        Assert.Equal("ok", result);
    }

    [Fact]
    public void Checkout_WithPercCoupon_AppliesDiscountAndTax()
    {
        var shop = new ShopManager();
        shop.Add("ItemA", 100);
        var total = shop.Checkout("PERC-10"); // 10% off then 21% tax: 100 -> 90 -> 108.9
        Assert.Equal(108.9, total);
    }

    [Fact]
    public void Export_ReturnsLowercaseCurrencyAndCsv()
    {
        var shop = new ShopManager();
        shop.Add("Pen", 2);
        var text = shop.Export();
        Assert.Contains("currency,usd", text);
        Assert.Contains("Pen:2", text);
    }
}
