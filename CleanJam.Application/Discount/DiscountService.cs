using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace CleanJam.Application.Discount
{
    public class DiscountService : IDiscountService
    {
        public void ApplyCartDiscount(ShoppingCart cart, Coupon? coupon)
        {
            var cartSubtotal = new Money(cart.Items.Sum(i => i.LineTotal), cart.Currency);
            var totalDiscountedAmount = coupon?.GetDiscountedAmount(cartSubtotal) ?? new Money(0, cart.Currency);

            foreach (var item in cart.Items)
            {
                // Allocate discount proportionally
                decimal allocatedDiscount = cartSubtotal > 0
                    ? (item.LineTotal / cartSubtotal) * totalDiscountedAmount
                    : 0;

                decimal discountedBase = item.LineTotal - totalDiscountedAmount;
            
                item.DiscountedLineTotal = new Money(discountedBase, cart.Currency);
            }
        }
    }
}
