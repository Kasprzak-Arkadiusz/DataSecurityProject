using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiLibrary.Persistence.Configuration
{
    internal class LastConnectionConfiguration : IEntityTypeConfiguration<LastConnection>
    {
        public void Configure(EntityTypeBuilder<LastConnection> builder)
        {
            builder.Property(l => l.DeviceType).HasMaxLength(32);
            builder.Property(l => l.BrowserName).HasMaxLength(32);
            builder.Property(l => l.PlatformName).HasMaxLength(32);
            builder.Property(l => l.City).HasMaxLength(64);
            builder.Property(l => l.Region).HasMaxLength(64);
            builder.Property(l => l.Country).HasMaxLength(64);
        }
    }
}