using BankClientgPRCService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankClientgPRCService.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id).HasName("user_pkey");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150).HasColumnName("name");
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(50).HasColumnName("phone");
            builder.HasIndex(x => x.Phone).IsUnique();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Salt).IsRequired();

            builder.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).HasConstraintName("user_from_role_id_fkey");
        }
    }
}
