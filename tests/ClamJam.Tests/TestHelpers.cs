using System.Reflection;
using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace ClamJam.Tests;

public static class TestHelpers
{
    public static ShoppingCart CreateCart(EnumCurrency currency, params CartItem[] items)
    {
        var cart = new ShoppingCart(currency);
        var field = typeof(ShoppingCart).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var dict = (Dictionary<Product, CartItem>)field.GetValue(cart)!;
        foreach (var item in items)
        {
            dict[item.Product] = item;
        }
        return cart;
    }
}


