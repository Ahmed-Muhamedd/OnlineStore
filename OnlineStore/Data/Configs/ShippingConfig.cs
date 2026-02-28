using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    public class ShippingConfig : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.CarrierName).HasColumnType("NVARCHAR(100)").IsRequired(true);
            builder.Property(p => p.TrackingNumber).HasColumnType("NVARCHAR(100)").IsRequired(true);
            builder.Property(p => p.ShippingStatus).HasColumnType("TINYINT").IsRequired(true);
            builder.Property(p => p.EstimatedDeliveryDate).HasColumnType("DATETIME").IsRequired(true);
            builder.Property(p => p.ActualDeliveryDate).HasColumnType("DATETIME").IsRequired(false);

        }
    }
}
