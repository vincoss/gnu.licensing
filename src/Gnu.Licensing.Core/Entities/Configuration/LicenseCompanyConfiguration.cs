using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Gnu.Licensing.Core.Entities.Configuration
{
    public class LicenseCompanyConfiguration : IEntityTypeConfiguration<LicenseCompany>
    {
        public void Configure(EntityTypeBuilder<LicenseCompany> builder)
        {
            builder.ToTable(nameof(LicenseCompany))
                   .HasIndex(x => new { x.CompanyName }).IsUnique();

            builder.ToTable(nameof(LicenseCompany))
                   .HasIndex(x => x.LicenseCompanyId).IsUnique();

            builder.HasKey(x => x.LicenseCompanyId);

            builder.Property(t => t.LicenseCompanyId)
                 .IsRequired();

            builder.Property(t => t.CompanyName)
                   .IsRequired();

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasDefaultValue(DateTime.UtcNow);

            builder.Property(x => x.CreatedByUser)
                   .IsRequired();
        }
    }
}
