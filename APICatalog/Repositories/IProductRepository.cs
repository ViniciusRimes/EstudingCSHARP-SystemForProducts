using APICatalog.Models;
using APICatalog.Pagination;

namespace APICatalog.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        PagedList<Product> GetProducts(ProductsParameters parameters);
        PagedList<Product> GetProductsFilteringForPrice(ProductsFilteringForPrice filters);
        IEnumerable<Product> GetProductsByCategory(int categoryId);
    }
}
