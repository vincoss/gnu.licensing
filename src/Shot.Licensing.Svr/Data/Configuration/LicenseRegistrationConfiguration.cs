using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace samplesl.Svr.Data.Configuration
{
    public class LicenseRegistrationConfiguration : IEntityTypeConfiguration<LicenseRegistration>
    {
        public void Configure(EntityTypeBuilder<LicenseRegistration> builder)
        {
            builder.ToTable(nameof(LicenseRegistration))
                  .HasIndex(x => new { x.ProductName, x.LicenseEmail }).IsUnique();

            builder.HasKey(x => x.LicenseRegistrationId);
            builder.Property(t => t.LicenseUuid)
                  .IsRequired();

            builder.Property(t => t.ProductName)
                   .IsRequired()
                   .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder.Property(t => t.LicenseName)
                .IsRequired()
                .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder.Property(t => t.LicenseEmail)
              .IsRequired()
              .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder.Property(t => t.IsActive)
           .IsRequired()
           .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("VARCHAR(64) COLLATE NOCASE");
        }
    }
}
