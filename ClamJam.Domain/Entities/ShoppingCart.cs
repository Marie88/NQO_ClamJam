using ClamJam.Domain.DomainModels;

namespace ClamJam.Domain.Entities
{
    public class ShoppingCart
    {
        private readonly Dictionary<Product, CartItem> _items = new();
        public EnumCurrency Currency { get; }

        public ShoppingCart(EnumCurrency currency = EnumCurrency.USD)
        {
            Currency = currency;
        }

        public IReadOnlyCollection<CartItem> Items => _items.Values.ToList().AsReadOnly();

        public int TotalItemCount => _items.Values.Sum(item => item.Quantity);

        public Money Subtotal => _items.Values
            .Aggregate(new Money(0, Currency), (sum, item) => sum.Add(item.LineTotal));

        /// <summary>
        /// The subtotal of the shopping cart after discounts, if there is no coupon applied,
        /// the discounted subtotal is equal to the subtotal
        /// </summary>
        public Money DiscountedSubtotal => _items.Values
           .Aggregate(new Money(0, Currency), (sum, item) => sum.Add(item.DiscountedLineTotal ?? item.LineTotal));
        public bool IsEmpty => _items.Count == 0;
    }
}
