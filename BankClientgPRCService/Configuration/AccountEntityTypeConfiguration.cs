using BankClientgPRCService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankClientgPRCService.Configuration
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(x => x.Id).HasName("account_pkey");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150).HasColumnName("name");
            builder.Property(x => x.Value).IsRequired().HasColumnName("value");
            builder.HasOne<User>().WithMany().HasForeignKey(c => c.UserId).HasConstraintName("account_from_user_id_fkey");

        }
    }
}
