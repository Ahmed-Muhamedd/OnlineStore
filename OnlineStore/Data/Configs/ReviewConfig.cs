using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.Text).HasColumnType("NVARCHAR(500)").IsRequired(true);
            builder.Property(p => p.Rating).HasColumnType("TINYINT").IsRequired(true);
            builder.Property(p => p.Date).HasColumnType("DATETIME").IsRequired(true);

        }
    }
}
