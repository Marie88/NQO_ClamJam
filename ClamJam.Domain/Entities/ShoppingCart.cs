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
        public void AddItem(Product product, int quantity = 1)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (product.Price.Currency != Currency)
                throw new InvalidOperationException($"Product currency {product.Price.Currency} does not match cart currency {Currency}");

            if (_items.TryGetValue(product, out var existingItem))
            {
                existingItem.AddQuantity(quantity);
            }
            else
            {
                _items[product] = new CartItem(product, quantity);
            }
        }

        public void RemoveItem(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _items.Remove(product);
        }

        public void UpdateItemQuantity(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
            {
                RemoveItem(product);
            }
            else if (_items.TryGetValue(product, out var item))
            {
                item.UpdateQuantity(quantity);
            }
            else
            {
                AddItem(product, quantity);
            }
        }
        public bool IsEmpty => _items.Count == 0;
    }
}
