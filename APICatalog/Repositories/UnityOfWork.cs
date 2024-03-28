using APICatalog.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Repositories
{
    public class UnityOfWork : IUnityOfWork, IDisposable
    {
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        public APICatalogContext _context;

        public UnityOfWork(APICatalogContext context)
        {
 
            _context = context;
        }
        public IProductRepository ProductRepository { get { return _productRepository = _productRepository ?? new ProductRepository(_context); } }
        public ICategoryRepository CategoryRepository { get { return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context); } }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    entry.Reload();
                }

                // Após recarregar os dados, tente salvar as alterações novamente
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
