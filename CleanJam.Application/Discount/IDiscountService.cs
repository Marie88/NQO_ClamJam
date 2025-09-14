using ClamJam.Domain.Entities;

namespace CleanJam.Application.Discount
{
    public interface IDiscountService
    {
        /// <summary>
        ///  Applies a cart discount and distributes it over the entire cart
        /// </summary>
        /// <param name="cart">Cart on which the discount should be applied</param>
        /// <param name="coupon">Coupon that is to be applied on the cart</param>
        void ApplyCartDiscount(ShoppingCart cart, Coupon? coupon);
    }
}
