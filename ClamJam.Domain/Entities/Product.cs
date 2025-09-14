using ClamJam.Domain.DomainModels;

namespace ClamJam.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Money Price { get; set; }
        public string Description { get; set; }
        public EnumProductType ProductType { get; set; }


        public Product(string name, Money price, string description, EnumProductType productType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be null or empty", nameof(name));

            Name = name;
            Price = price;
            Description = description;
            ProductType = productType;
        }

        public Product(Guid id, string name, Money price, string description, EnumProductType productType)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be null or empty", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be null or empty", nameof(name));

            Id = id;
            Name = name;
            Price = price;
            Description = description;
            ProductType = productType;
        }



        public override string ToString() => $"{Name} - {Price}";
    }

    public enum EnumProductType
    {
        Food,
        NonFood
    }
}
