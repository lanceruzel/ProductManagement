using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Models.Product> Product { get; set; }
        public DbSet<Models.Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothes" },
                new Category { Id = 3, Name = "Grocery" },
                new Category { Id = 4, Name = "Books" },
                new Category { Id = 5, Name = "Furniture" },
                new Category { Id = 6, Name = "Toys" },
                new Category { Id = 7, Name = "Tools" },
                new Category { Id = 8, Name = "Automotive" },
                new Category { Id = 9, Name = "Sports" },
                new Category { Id = 10, Name = "Music" },
                new Category { Id = 11, Name = "Movies" },
                new Category { Id = 12, Name = "Health" },
                new Category { Id = 13, Name = "Beauty" },
                new Category { Id = 14, Name = "Home" },
                new Category { Id = 15, Name = "Garden" },
                new Category { Id = 16, Name = "Pet" },
                new Category { Id = 17, Name = "Baby" },
                new Category { Id = 18, Name = "Industrial" },
                new Category { Id = 19, Name = "Handmade" },
                new Category { Id = 20, Name = "Software" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
