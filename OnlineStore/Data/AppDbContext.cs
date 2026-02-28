using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Models;

namespace OnlineStore.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderItem> OrderItems { set; get; }
        public DbSet<Payment> Payments { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<Review> Reviews { set; get; }
        public DbSet<Shipping> Shippings { set; get; }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            SeedRoles(modelBuilder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "F2AE8C42-51E8-44AD-B839-C48A6C5D88AC",
                    Name = "admin",
                    ConcurrencyStamp = "A5FB172A-1352-4F4A-9BF7-22E717C34712",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "39D4E833-6236-4415-A95D-D52B702E8E72",
                    Name = "customer",
                    ConcurrencyStamp = "0127058D-1289-4F79-93E0-ABAB3F63D46C",
                    NormalizedName = "CUSTOMER"
                }
                );
        }
    }
}
