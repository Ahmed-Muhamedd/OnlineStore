using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    // Config relation with (OrderItems , Payment , Shipping , Customer) 

    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.CreatedAt).HasColumnType("SMALLDATETIME").HasDefaultValueSql("GetDate()").IsRequired(true);
            builder.Property(p => p.TotalAmount).HasColumnType("SMALLMONEY").IsRequired(true);
            builder.Property(p => p.Status).HasColumnType("SMALLINT").IsRequired(true);

            // OrderItems Relation Config
            builder.HasMany(order => order.OrderItems)
                .WithOne(orderItem => orderItem.Order)
                .HasForeignKey(fk => fk.OrderID).IsRequired(true);

            // Payment Relation Config
            builder.HasOne(order => order.Payment)
                .WithOne(payment => payment.Order)
                .HasForeignKey<Payment>(fk => fk.OrderID).IsRequired(true);

            // Shipping Relation Config
            builder.HasOne(order => order.Shipping)
                .WithOne(shipping => shipping.Order)
                .HasForeignKey<Shipping>(fk => fk.OrderID).IsRequired(true);

            // Customer Relation Config
            builder.HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(fk => fk.CustomerID).IsRequired(true);

        }
    }
}
