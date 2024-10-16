using BankClientgPRCService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService.Configuration
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");        
            builder.HasKey(x => x.Id).HasName("role_pkey");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
