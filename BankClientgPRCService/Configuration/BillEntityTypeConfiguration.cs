using BankClientgPRCService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankClientgPRCService.Configuration
{
    public class BillEntityTypeConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bill");
            builder.HasKey(x => x.Id).HasName("bill_pkey");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150).HasColumnName("name");
            builder.Property(x => x.Value).IsRequired().HasColumnName("value");
            builder.HasOne<User>().WithMany().HasForeignKey(c => c.UserId).HasConstraintName("bill_from_user_id_fkey");

        }
    }
}
