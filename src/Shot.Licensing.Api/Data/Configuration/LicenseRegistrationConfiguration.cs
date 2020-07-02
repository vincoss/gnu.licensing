using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Shot.Licensing.Api.Data.Configuration
{
    public class LicenseRegistrationConfiguration : IEntityTypeConfiguration<LicenseRegistration>
    {
        public void Configure(EntityTypeBuilder<LicenseRegistration> builder)
        {
            builder.ToTable(nameof(LicenseRegistration))
                  .HasIndex(x => new { x.LicenseRegistrationId, x.LicenseName, x.LicenseEmail, x.IsActive }).IsUnique();

            builder.ToTable(nameof(LicenseRegistration))
                .HasIndex(x => new { x.LicenseUuid }).IsUnique();

            builder.HasKey(x => x.LicenseRegistrationId);

            builder.Property(t => t.LicenseRegistrationId)
                   .IsRequired()
                   .HasColumnType("INTEGER");

            builder.Property(t => t.LicenseUuid)
                   .IsRequired()
                   .HasDefaultValue(Guid.NewGuid())
                   .HasColumnType("VARCHAR(36)");
            
            builder.Property(t => t.ProductUuid)
                   .IsRequired()
                   .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.LicenseName)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder.Property(t => t.LicenseEmail)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnType("BOOLEAN");

            builder.Property(t => t.Expire)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasDefaultValue(DateTime.UtcNow)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
