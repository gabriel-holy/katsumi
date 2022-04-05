using Microsoft.EntityFrameworkCore;
using PosterrApp.Models;

namespace PosterrApp.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
