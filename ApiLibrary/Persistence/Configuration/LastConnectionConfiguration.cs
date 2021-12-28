using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiLibrary.Persistence.Configuration
{
    internal class LastConnectionConfiguration : IEntityTypeConfiguration<LastConnection>
    {
        public void Configure(EntityTypeBuilder<LastConnection> builder)
        {
            builder.Property(l => l.DeviceDetails).HasMaxLength(128);
            builder.Property(l => l.Location).HasMaxLength(128);
        }
    }
}