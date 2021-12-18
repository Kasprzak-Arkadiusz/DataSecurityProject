using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configuration
{
    internal class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
    {
        public void Configure(EntityTypeBuilder<PasswordReset> builder)
        {
            builder.Property(p => p.ResetToken).HasMaxLength(128).IsRequired();
            builder.Property(p => p.ValidTo).IsRequired();
        }
    }
}