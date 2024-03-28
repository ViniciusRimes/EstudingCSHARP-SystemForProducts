using APICatalog.Models;
using APICatalog.Pagination;

namespace APICatalog.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        PagedList<Category> GetCategories(CategoriesParameters parameters);
    }
}
