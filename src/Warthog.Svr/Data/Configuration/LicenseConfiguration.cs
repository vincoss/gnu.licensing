using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Warthog.Api.Data.Configuration
{
    public class LicenseConfiguration : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.ToTable(nameof(License))
                   .HasIndex(x => new { x.LicenseId, x.IsActive }).IsUnique();

            builder.HasKey(x => x.LicenseId);

            builder.Property(t => t.LicenseId)
                .IsRequired()
                .HasColumnType("INTEGER");

            builder.Property(t => t.LicenseUuid)
                   .IsRequired()
                   .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.ProductUuid)
                .IsRequired()
                .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.LicenseString)
                   .IsRequired()
                   .HasColumnType("NVARCHAR COLLATE NOCASE");

            builder.Property(t => t.LicenseAttributes)
                   .HasColumnType("NVARCHAR COLLATE NOCASE");

            builder.Property(t => t.LicenseChecksum)
                .IsRequired()
                .HasColumnType("NVARCHAR");

            builder.Property(t => t.AttributesChecksum)
                .HasColumnType("NVARCHAR");

            builder.Property(t => t.ChecksumType)
              .IsRequired()
              .HasColumnType("VARCHAR(12) COLLATE NOCASE");

            builder.Property(t => t.IsActive)
                      .IsRequired()
                      .HasDefaultValue(true)
                      .HasColumnType("BOOLEAN");

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasColumnType("DATETIME");

            builder.Property(x => x.ModifiedDateTimeUtc)
               .IsRequired()
               .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(64) COLLATE NOCASE");

            builder.Property(x => x.ModifiedByUser)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
