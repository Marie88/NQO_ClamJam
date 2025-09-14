using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace CleanJam.Application.Tax
{   /// <summary>
    /// Defines a contract for applying tax rules to products based on their type or category.
    /// </summary>
    public interface ITaxStrategy
    {
        public EnumVatType VatType { get; set; }
        public Percentage Rate { get; }
        /// <summary>
        /// Returns the tax to be paid for a particular item in the shopping cart
        /// </summary>
        /// <param name="item">The cart item for which tax needs to be determined.</param>
        Money CalculateTax(CartItem item);
    }
}
