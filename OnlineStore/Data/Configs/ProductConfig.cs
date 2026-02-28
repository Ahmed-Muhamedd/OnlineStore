using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    // Config relation with (ProductImages , ProductCategory , Review , OrderItem) 
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.Name).HasColumnType("NVARCHAR(100)").IsRequired(true);
            builder.Property(p => p.Description).HasColumnType("NVARCHAR(500)").IsRequired(true);
            builder.Property(p => p.Price).HasColumnType("SMALLMONEY").IsRequired(true);
            builder.Property(P => P.QuantityInStock).HasColumnType("INT").IsRequired(true);

            // Product Images Relation Config
            builder.HasMany(product => product.Images)
                .WithOne(image => image.Product)
                .HasForeignKey(fk => fk.ProductID)
                .IsRequired(true);

            // Product Category Relation Config
            builder.HasOne(product => product.ProductCategory)
                .WithMany(category => category.Products)
                .HasForeignKey(fk => fk.CategoryID)
                .IsRequired(true);

            // Review Relation Config
            builder.HasMany(product => product.Reviews)
                .WithOne(review => review.Product)
                .HasForeignKey(fk => fk.ProductID)
                .IsRequired(true);

            // Order Item Relation Config
            builder.HasMany(product => product.OrderItems)
            .WithOne(orderItem => orderItem.Product)
            .HasForeignKey(fk => fk.ProductID)
            .IsRequired(true);

        }
    }
}
