using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;

namespace ClamJam.Domain.Dtos
{
    public record AddProductRequest(string Name, string Description, EnumProductType ProductType, Money Price);
    public record DeleteProductRequest(Guid ProductId);
    public record UpdateProductRequest(Guid Id, string Name, string Description, EnumProductType ProductType, Money Price);
}
