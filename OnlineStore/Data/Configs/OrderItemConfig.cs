using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Ef.Configs
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(p => p.Quantity).HasColumnType("INT").IsRequired(true);
            builder.Property(p => p.Price).HasColumnType("SMALLMONEY").IsRequired(true);
            builder.Property(p => p.TotalItemsPrice).HasColumnType("SMALLMONEY").IsRequired(true);

        }
    }
}
