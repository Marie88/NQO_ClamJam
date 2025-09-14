using ClamJam.Domain.Entities;

namespace CleanJam.Application.Tax
{
    public interface ITaxService
    {
        /// <summary>
        /// Compute the tax to be paid for a list of cart items 
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>Total tax to be paid for the provided list of items</returns>
        decimal ComputeTax(IReadOnlyCollection<CartItem> cartItems);
        /// <summary>
        /// Returns the tax strategy applied on an item in the cart
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        ITaxStrategy GetVatTypeForItem(CartItem cartItem);
    }
}
