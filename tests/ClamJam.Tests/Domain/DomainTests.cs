using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
public class CurrencyTests
{
    [Theory]
    [InlineData(EnumCurrency.USD, "usd")]
    [InlineData(EnumCurrency.EUR, "eur")]
    [InlineData(EnumCurrency.GBP, "gbp")]
    [InlineData(EnumCurrency.CAD, "cad")]
    [InlineData(EnumCurrency.AUD, "aud")]
    public void ToCode_ReturnsExpected(EnumCurrency currency, string code)
    {
        Assert.Equal(code, currency.ToCode());
    }

    [Theory]
    [InlineData("usd", EnumCurrency.USD)]
    [InlineData("EUR", EnumCurrency.EUR)]
    [InlineData("GbP", EnumCurrency.GBP)]
    public void FromCode_ReturnsExpected(string code, EnumCurrency expected)
    {
        Assert.Equal(expected, CurrencyExtensions.FromCode(code));
    }

    [Fact]
    public void FromCode_Invalid_Throws()
    {
        Assert.Throws<ArgumentException>(() => CurrencyExtensions.FromCode("xyz"));
    }
}

public class MoneyTests
{
    [Fact]
    public void Constructor_Negative_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Money(-1, EnumCurrency.USD));
    }

    [Fact]
    public void Add_SameCurrency_ReturnsSum()
    {
        var m1 = new Money(5, EnumCurrency.USD);
        var m2 = new Money(10, EnumCurrency.USD);

        var result = m1.Add(m2);

        Assert.Equal(15m, result.Amount);
    }

    [Fact]
    public void Add_DifferentCurrency_Throws()
    {
        var m1 = new Money(5, EnumCurrency.USD);
        var m2 = new Money(10, EnumCurrency.EUR);

        Assert.Throws<InvalidOperationException>(() => m1.Add(m2));
    }

    [Fact]
    public void Multiply_PositiveFactor()
    {
        var money = new Money(10, EnumCurrency.USD);
        var result = money.Multiply(2);

        Assert.Equal(20m, result.Amount);
    }

    [Fact]
    public void Multiply_NegativeFactor_Throws()
    {
        var money = new Money(10, EnumCurrency.USD);
        Assert.Throws<ArgumentException>(() => money.Multiply(-1));
    }

    [Fact]
    public void Subtract_SameCurrency_ReturnsDifference()
    {
        var m1 = new Money(10, EnumCurrency.USD);
        var m2 = new Money(3, EnumCurrency.USD);

        var result = m1.Subtract(m2);

        Assert.Equal(7m, result.Amount);
    }

    [Fact]
    public void Subtract_DifferentCurrency_Throws()
    {
        var m1 = new Money(10, EnumCurrency.USD);
        var m2 = new Money(3, EnumCurrency.EUR);

        Assert.Throws<InvalidOperationException>(() => m1.Subtract(m2));
    }
}

public class PercentageTests
{
    [Fact]
    public void Factor_ComputedCorrectly()
    {
        var p = new Percentage(50);
        Assert.Equal(0.5m, p.Factor);
    }

    [Fact]
    public void Constructor_OutOfRange_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Percentage(150));
    }

    [Fact]
    public void ImplicitConversions_Work()
    {
        Percentage p = 25m;
        decimal value = p;
        Assert.Equal(25m, value);
    }
}

public class CartItemTests
{
    [Fact]
    public void Constructor_InvalidQuantity_Throws()
    {
        var product = new Product("p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        Assert.Throws<ArgumentException>(() => new CartItem(product, 0));
    }

    [Fact]
    public void LineTotal_ComputedFromQuantityAndPrice()
    {
        var product = new Product("p", new Money(2, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 3);
        Assert.Equal(6m, item.LineTotal.Amount);
    }

    [Fact]
    public void UpdateQuantity_ChangesQuantity()
    {
        var product = new Product("p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        item.UpdateQuantity(5);
        Assert.Equal(5, item.Quantity);
    }

    [Fact]
    public void AddQuantity_IncreasesQuantity()
    {
        var product = new Product("p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        item.AddQuantity(2);
        Assert.Equal(3, item.Quantity);
    }

    [Fact]
    public void UpdateQuantity_Negative_Throws()
    {
        var product = new Product("p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        Assert.Throws<ArgumentException>(() => item.UpdateQuantity(0));
    }

    [Fact]
    public void AddQuantity_Negative_Throws()
    {
        var product = new Product("p", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1);
        Assert.Throws<ArgumentException>(() => item.AddQuantity(0));
    }
}

public class CouponTests
{
    [Fact]
    public void FreeCoupon_ReturnsFullAmount()
    {
        var coupon = new FreeCoupon();
        var subtotal = new Money(100, EnumCurrency.USD);
        var result = coupon.GetDiscountedAmount(subtotal);
        Assert.Equal(100m, result.Amount);
    }

    [Fact]
    public void PercentageCoupon_DiscountsProperly()
    {
        var coupon = new PercentageCoupon(new Percentage(10));
        var subtotal = new Money(200, EnumCurrency.USD);
        var result = coupon.GetDiscountedAmount(subtotal);
        Assert.Equal(20m, result.Amount);
    }

    [Fact]
    public void CouponFactory_CreatesFreeCoupon()
    {
        var coupon = CouponFactory.Create(EnumCouponCode.Free, null);
        Assert.IsType<FreeCoupon>(coupon);
    }

    [Fact]
    public void CouponFactory_CreatesPercentageCoupon()
    {
        var coupon = CouponFactory.Create(EnumCouponCode.Percentage, new Percentage(5));
        Assert.IsType<PercentageCoupon>(coupon);
    }

    [Fact]
    public void CouponFactory_PercentageWithoutValue_Throws()
    {
        Assert.Throws<ArgumentException>(() => CouponFactory.Create(EnumCouponCode.Percentage, null));
    }

    [Fact]
    public void CouponFactory_NullCode_ReturnsNull()
    {
        var coupon = CouponFactory.Create(null, new Percentage(10));
        Assert.Null(coupon);
    }
}

