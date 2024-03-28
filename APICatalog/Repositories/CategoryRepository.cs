using APICatalog.Context;
using APICatalog.Models;
using APICatalog.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(APICatalogContext context) : base(context)
        {
        }

        public PagedList<Category> GetCategories(CategoriesParameters parameters)
        {
            var categories = GetAll().OrderBy(p => p.CategoryId).AsQueryable();
            var categoriesOrdered = PagedList<Category>.ToPagedList(categories, parameters.PageNumber, parameters.PageSize);
            return categoriesOrdered;
        }
    }
}
