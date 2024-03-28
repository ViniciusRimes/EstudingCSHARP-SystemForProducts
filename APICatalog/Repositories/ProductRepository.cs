using APICatalog.Context;
using APICatalog.Models;
using APICatalog.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(APICatalogContext context) : base(context)
        {
        }
          
        public PagedList<Product> GetProducts(ProductsParameters parameters)
        {
            var products = GetAll().OrderBy(p => p.ProductId).AsQueryable();
            var productsOrdered = PagedList<Product>.ToPagedList(products, parameters.PageNumber, parameters.PageSize);
            return productsOrdered;
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return GetAll().Where(c => c.CategoryId == categoryId);
        }

        public PagedList<Product> GetProductsFilteringForPrice(ProductsFilteringForPrice filters)
        {
            var products = GetAll().AsQueryable();
            if (filters.Price.HasValue && !string.IsNullOrEmpty(filters.PriceCritery))
            {
                if (filters.PriceCritery.Equals("bigger", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price > filters.Price.Value).OrderBy(p => p.ProductId);
                }
                else if (filters.PriceCritery.Equals("smaller", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < filters.Price.Value).OrderBy(p => p.ProductId);
                }
                else if (filters.PriceCritery.Equals("equal", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == filters.Price.Value).OrderBy(p => p.ProductId);
                }
            }
            return PagedList<Product>.ToPagedList(products, filters.PageNumber, filters.PageSize);
        }
    }
}
