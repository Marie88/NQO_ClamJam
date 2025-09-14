using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace CleanJam.Application.Pricing;

public interface IPricingService
{
    /// <summary>
    ///  Calculate the total price of the cart to be paid by the cusomer
    /// </summary>
    /// <param name="cart">The current cart</param>
    /// <param name="coupon">Coupon to be applied to the current cart,if left empty, no discount is applied</param>
    /// <returns>Total amount and currency to be paid by the customer</returns>
    Money CalculateTotal(ShoppingCart cart, Coupon? coupon = null);
}