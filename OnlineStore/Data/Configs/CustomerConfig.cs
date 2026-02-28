using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Models;
using OnlineStore.Data;

namespace OnlineStore.Ef.Configs
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(pk => pk.UserID);
         

            builder.HasMany(customer => customer.Reviews)
                .WithOne(review => review.Customer)
                .HasForeignKey(fk => fk.CustomerID).IsRequired(true);

            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Customer>(fk => fk.UserID).IsRequired(true);

        }
    }
}
