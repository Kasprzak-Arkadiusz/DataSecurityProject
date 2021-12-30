using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiLibrary.Persistence.Configuration
{
    internal class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
    {
        public void Configure(EntityTypeBuilder<PasswordReset> builder)
        {
            builder.Property(p => p.ResetToken).HasMaxLength(256).IsRequired();
            builder.Property(p => p.ValidTo).IsRequired();
        }
    }
}