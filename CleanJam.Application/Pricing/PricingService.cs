using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Discount;
using CleanJam.Application.Tax;

namespace CleanJam.Application.Pricing;

public class PricingService : IPricingService
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;

    public PricingService(ITaxService taxService, IDiscountService discountService)
    {
        _taxService = taxService;
        _discountService = discountService;
    }

    public Money CalculateTotal(ShoppingCart cart, Coupon? coupon = null)
    {
        if (cart.IsEmpty)
            return new Money(0, cart.Currency);

        var subtotalBeforeTaxes = cart.Subtotal;

        if(coupon != null)
        {
            _discountService.ApplyCartDiscount(cart, coupon);
        }

        var afterDiscountSubtotal = cart.DiscountedSubtotal;
        var taxAmount = new Money(_taxService.ComputeTax(cart.Items), cart.Currency);
        var totalAfterTaxes = new Money(afterDiscountSubtotal + taxAmount, cart.Currency);
        return totalAfterTaxes;
    }
}