using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;

namespace OnlineStore.Ef.Configs
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(pk => pk.ID);
            builder.Property(p => p.PaymentMethod).HasColumnType("NVARCHAR(50)").IsRequired(true);
            builder.Property(p => p.Amount).HasColumnType("SMALLMONEY").IsRequired(true);
            builder.Property(p => p.TransactionDate).HasColumnType("DATETIME").IsRequired(true);

        }
    }
}
