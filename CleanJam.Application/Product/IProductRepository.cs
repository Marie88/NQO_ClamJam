namespace CleanJam.Application.Product
{
    using ClamJam.Domain.Entities;
    public interface IProductRepository
    {
        /// <summary>
        ///  Returns the product matching the supplied Id.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Product matching the supplied id. If none match, null is returned</returns>
        public Product? GetProductAsync(Guid productId);
        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <returns>All products</returns>
        public IList<Product> GetProductsAsync();
        /// <summary>
        /// Save or update a product. If no Id was provided, a new product will be created.
        /// </summary>
        /// <param name="product">Product to be saved or updated</param>
        Task<Product> SaveOrUpdateProductAsync(Product product);
        /// <summary>
        /// Delete a product by its id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        Task DeleteProductAsync(Guid productId);
    }

}
