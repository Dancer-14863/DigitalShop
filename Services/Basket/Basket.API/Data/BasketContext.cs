using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class BasketContext : DbContext
    {
        public BasketContext(DbContextOptions<BasketContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Basket>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<BasketProduct>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Models.Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
    }
    
    
}