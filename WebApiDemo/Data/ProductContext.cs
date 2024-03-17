using Microsoft.EntityFrameworkCore;

namespace WebApiDemo.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products{ get; set; }  
        public DbSet<Category> Categories{ get; set; }  

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new() { Id = 1, Name = "Elektronik"},
                new() { Id = 2, Name = "Giyim"},
            });

            modelBuilder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Product>().HasData(new Product[]
            {
                new(){Id = 1, Name="MSI Creator Z16", Price=99000, Stock = 1000, CreatedDate = DateTime.Now.AddDays(-20), ImagePath="", CategoryId = 1},
                new(){Id = 2, Name="Asus ROG Strix G16", Price=65000, Stock = 733, CreatedDate = DateTime.Now.AddDays(-10), ImagePath = "", CategoryId = 1},
                new(){Id = 3, Name="Gigabyte Aorus 15", Price=53000, Stock = 333, CreatedDate = DateTime.Now.AddDays(-1), ImagePath = "", CategoryId = 1},
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
