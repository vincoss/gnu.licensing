using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shot.Licensing.Api.Data.Configuration
{
    public class LicenseProductConfiguration : IEntityTypeConfiguration<LicenseProduct>
    {
        public void Configure(EntityTypeBuilder<LicenseProduct> builder)
        {
            builder.ToTable(nameof(LicenseProduct))
                   .HasIndex(x => new { x.LicenseProductId, x.ProductName }).IsUnique();

            builder.ToTable(nameof(LicenseProduct))
                   .HasIndex(x => x.ProductUuid).IsUnique();

            builder.ToTable(nameof(LicenseProduct))
                   .HasIndex(x => x.ProductName).IsUnique();

            builder.HasKey(x => x.LicenseProductId);

            builder.Property(t => t.LicenseProductId)
                 .IsRequired()
                 .HasColumnType("INTEGER");

            builder.Property(t => t.ProductUuid)
                   .IsRequired()
                   .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.ProductName)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder.Property(t => t.ProductDescription)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(1024) COLLATE NOCASE");

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
