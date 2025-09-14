using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Tax;

namespace ClamJam.Tests;

public class TaxServiceTests
{
    [Fact]
    public void ComputeTax_SumsStrategies()
    {
        var foodStrategy = new StandardVatStrategy(10) { VatType = EnumVatType.Food };
        var nonFoodStrategy = new StandardVatStrategy(20) { VatType = EnumVatType.NonFood };
        var service = new TaxService(new Dictionary<EnumProductType, ITaxStrategy>
        {
            [EnumProductType.Food] = foodStrategy,
            [EnumProductType.NonFood] = nonFoodStrategy
        });

        var food = new Product("apple", new Money(100, EnumCurrency.USD), "d", EnumProductType.Food);
        var nonFood = new Product("soap", new Money(50, EnumCurrency.USD), "d", EnumProductType.NonFood);
        var cart = TestHelpers.CreateCart(EnumCurrency.USD, new CartItem(food, 1), new CartItem(nonFood, 1));

        var total = service.ComputeTax(cart.Items);

        Assert.Equal(20m, total);
    }

    [Fact]
    public void GetVatTypeForItem_ReturnsStrategy()
    {
        var strategy = new StandardVatStrategy(5) { VatType = EnumVatType.Food };
        var service = new TaxService(new Dictionary<EnumProductType, ITaxStrategy> { [EnumProductType.Food] = strategy });
        var product = new Product("apple", new Money(1, EnumCurrency.USD), "d", EnumProductType.Food);
        var cartItem = new CartItem(product, 1);

        var result = service.GetVatTypeForItem(cartItem);

        Assert.Same(strategy, result);
    }
}

public class TaxStrategyTests
{
    [Fact]
    public void StandardVatStrategy_UsesDiscountedLineTotal()
    {
        var product = new Product("apple", new Money(100, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1) { DiscountedLineTotal = new Money(80, EnumCurrency.USD) };
        var strategy = new StandardVatStrategy(10);

        var tax = strategy.CalculateTax(item);

        Assert.Equal(8m, tax.Amount);
    }
}

