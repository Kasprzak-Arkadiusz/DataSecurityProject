using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configuration
{
    internal class SecretConfiguration : IEntityTypeConfiguration<Secret>
    {
        public void Configure(EntityTypeBuilder<Secret> builder)
        {
            builder.Property(s => s.ServiceName).HasMaxLength(50).IsRequired();
            builder.Property(s => s.Password).HasMaxLength(128).IsRequired();
            builder.Property(s => s.Iv).HasMaxLength(128).IsRequired();
        }
    }
}