using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogDetail> BlogsDetail { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Detail)
                .WithOne(d => d.Blog)
                .HasForeignKey<BlogDetail>(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogDetail>()
                .HasIndex(d => d.BlogId)
                .IsUnique();
        }


    }


}
