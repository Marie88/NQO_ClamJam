using ClamJam.Domain.DomainModels;

namespace ClamJam.Domain.Entities
{
    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }
        public Money LineTotal => Product.Price.Multiply(Quantity);
        public Money? DiscountedLineTotal { get; set; }

        public CartItem(Product product, int quantity = 1)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity;
        }

        /// <summary>
        /// Update quantity of the cart item
        /// </summary>
        /// <param name="newQuantity">New quantity</param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(newQuantity));

            Quantity = newQuantity;
        }

        /// <summary>
        /// Add on top of the quantity of the cart item
        /// </summary>
        /// <param name="newQuantity">Additional units to be added</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddQuantity(int additionalQuantity)
        {
            if (additionalQuantity <= 0)
                throw new ArgumentException("Additional quantity must be positive", nameof(additionalQuantity));

            Quantity += additionalQuantity;
        }
    }
}
