using APICatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Context;
public class APICatalogContext : DbContext
{
    public APICatalogContext(DbContextOptions<APICatalogContext> options) : base(options)
    {

    }
    public DbSet <Category>? Categories { get; set; }
    public DbSet <Product>? Products { get; set; }

}
