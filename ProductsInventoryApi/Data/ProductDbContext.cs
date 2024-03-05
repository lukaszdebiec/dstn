using Microsoft.EntityFrameworkCore;
using ProductsInventoryApi.Models;

namespace ProductsInventoryApi.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}