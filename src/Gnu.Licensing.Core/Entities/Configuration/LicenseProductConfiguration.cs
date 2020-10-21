using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace Gnu.Licensing.Core.Entities.Configuration
{
    public class LicenseProductConfiguration : IEntityTypeConfiguration<LicenseProduct>
    {
        public void Configure(EntityTypeBuilder<LicenseProduct> builder)
        {
            builder.ToTable(nameof(LicenseProduct))
                   .HasIndex(x => new { x.ProductName }).IsUnique();

            builder.ToTable(nameof(LicenseProduct))
                   .HasIndex(x => x.LicenseProductId).IsUnique();

            builder.HasKey(x => x.LicenseProductId);

            builder.Property(t => t.LicenseProductId)
                 .IsRequired();

            builder.Property(t => t.CompanyId)
                 .IsRequired();

            builder.Property(t => t.ProductName)
                   .IsRequired();

            builder.Property(t => t.ProductDescription)
                   .IsRequired();

            builder.Property(x => x.SignKeyName)
                  .IsRequired();

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasDefaultValue(DateTime.UtcNow);

            builder.Property(x => x.CreatedByUser)
                   .IsRequired();
        }
    }
}
