using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.Name).HasColumnType("NVARCHAR(100)").IsRequired(true);

        }
    }
}
