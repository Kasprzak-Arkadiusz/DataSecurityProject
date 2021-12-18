using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(128).IsRequired();
            builder.Property(u => u.MasterPassword).HasMaxLength(128).IsRequired();
            
            builder.HasOne(u => u.PasswordReset)
                .WithOne(p => p.User)
                .HasForeignKey<PasswordReset>(p => p.UserId);

            builder.HasOne(u => u.LoginFailure)
                .WithOne(l => l.User)
                .HasForeignKey<LoginFailure>(l => l.UserId);

            builder.HasMany(u => u.LastConnections)
                .WithOne(l => l.User);

            builder.HasMany(u => u.Secrets)
                .WithOne(s => s.User);
        }
    }
}