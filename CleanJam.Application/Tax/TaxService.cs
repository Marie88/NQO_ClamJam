using ClamJam.Domain.Entities;

namespace CleanJam.Application.Tax
{

    public class TaxService : ITaxService
    {
        private readonly Dictionary<EnumProductType, ITaxStrategy> _strategies;

        public TaxService(Dictionary<EnumProductType, ITaxStrategy> strategies) =>
            _strategies = strategies;

        public decimal ComputeTax(IReadOnlyCollection<CartItem> cartItems) =>
            cartItems.Sum(item =>
                _strategies[item.Product.ProductType].CalculateTax(item));

        public ITaxStrategy GetVatTypeForItem(CartItem cartItem)
        {
            return _strategies[cartItem.Product.ProductType];
        }
    }
}
