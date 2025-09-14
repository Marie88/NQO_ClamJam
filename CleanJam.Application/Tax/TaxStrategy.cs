using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace CleanJam.Application.Tax
{
    public enum EnumVatType
    {
        Food,
        NonFood
    }
    public class StandardVatStrategy : ITaxStrategy
    {
        public EnumVatType VatType { get; set; }
        public Percentage Rate { get;}
        public StandardVatStrategy(decimal rate) => Rate = rate;

        public Money CalculateTax(CartItem item) =>
            new Money((item.DiscountedLineTotal ?? item.LineTotal) * Rate.Factor, item.LineTotal.Currency);
    }
}
