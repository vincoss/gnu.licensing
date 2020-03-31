using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shot.Licensing.Api.Data.Configuration
{
    public class LicenseRegistrationConfiguration : IEntityTypeConfiguration<LicenseRegistration>
    {
        public void Configure(EntityTypeBuilder<LicenseRegistration> builder)
        {
            builder.ToTable(nameof(LicenseRegistration))
                  .HasIndex(x => new { x.LicenseRegistrationId, x.LicenseProductId, x.LicenseName, x.LicenseEmail, x.IsActive }).IsUnique();

            builder.ToTable(nameof(LicenseRegistration))
                .HasIndex(x => x.LicenseUuid).IsUnique();

            builder.HasKey(x => x.LicenseRegistrationId);

            builder.Property(t => t.LicenseRegistrationId)
                   .IsRequired()
                   .HasColumnType("INTEGER");

            builder.Property(t => t.LicenseUuid)
                   .IsRequired()
                   .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.LicenseProductId)
                   .IsRequired()
                   .HasColumnType("INTEGER");

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

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
