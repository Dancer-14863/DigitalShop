using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Order>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
    }
}