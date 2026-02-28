using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.Url).HasColumnType("NVARCHAR(100)").IsRequired(true);

        }
    }
}
