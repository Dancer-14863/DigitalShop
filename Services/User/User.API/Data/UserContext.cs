using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using User.API.Misc;
using User.API.Models;

namespace User.API.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seeding
            modelBuilder.Entity<Role>().HasData
            (
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Vendor" },
                new Role { Id = 3, Name = "User" }
            );
            // have to reset the id sequence manually because of psql
            modelBuilder.Entity<Role>()
                .Property(r => r.Id)
                .HasIdentityOptions(startValue: 4);
            
            modelBuilder.Entity<Account>().HasData
            (
                new Account
                {
                    Id = 1,
                    Name = "admin",
                    Email = "admin@gmail.com",
                    Password = new PasswordHelper().HashPassword("@dmiN123"),
                    RoleId = 1
                }
            );
            modelBuilder.Entity<Account>()
                .Property(r => r.Id)
                .HasIdentityOptions(startValue: 2);
            modelBuilder.Entity<Account>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
    
    
}