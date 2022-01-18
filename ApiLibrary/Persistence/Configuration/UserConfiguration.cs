using ApiLibrary.Common;
using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiLibrary.Persistence.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName).HasMaxLength(Constants.MaxUsernameLength).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(Constants.MaxPasswordLength).IsRequired();
            builder.Property(u => u.MasterPassword).HasMaxLength(Constants.MaxMasterPasswordLength).IsRequired();
            
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