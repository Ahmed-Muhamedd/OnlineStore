using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Data;

namespace OnlineStore.Ef.Configs
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
       
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired(true);
            builder.Property(p => p.SecondName).HasMaxLength(100).IsRequired(true);
        }
    }
}
