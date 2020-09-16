using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Gnu.Licensing.Svr.Data.Configuration
{
    public class LicenseCompanyConfiguration : IEntityTypeConfiguration<LicenseCompany>
    {
        public void Configure(EntityTypeBuilder<LicenseCompany> builder)
        {
            builder.ToTable(nameof(LicenseCompany))
                   .HasIndex(x => new { x.LicenseCompanyId, x.CompanyName }).IsUnique();

            builder.ToTable(nameof(LicenseCompany))
                   .HasIndex(x => x.CompanyUuid).IsUnique();

            builder.ToTable(nameof(LicenseCompany))
                   .HasIndex(x => x.CompanyName).IsUnique();

            builder.HasKey(x => x.LicenseCompanyId);

            builder.Property(t => t.LicenseCompanyId)
                 .IsRequired()
                 .HasColumnType("INTEGER");

            builder.Property(t => t.CompanyUuid)
                   .IsRequired()
                   .HasDefaultValue(Guid.NewGuid())
                   .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.CompanyName)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

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
