using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace samplesl.Svr.Data.Configuration
{
    public class LicenseConfiguration : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.ToTable(nameof(License))
                   .HasIndex(x => new { x.LicenseId, x.IsActive }).IsUnique();

            builder.HasKey(x => x.LicenseId);
            builder.Property(t => t.LicenseUuid)
                  .IsRequired();

            builder.Property(t => t.LicenseString)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(max) COLLATE NOCASE");

            builder.Property(t => t.Checksum)
                .IsRequired()
                .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder.Property(t => t.ChecksumType)
              .IsRequired()
              .HasColumnType("VARCHAR(12) COLLATE NOCASE");

            builder.Property(t => t.IsActive)
           .IsRequired()
           .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasColumnType("DATETIME");

            builder.Property(x => x.ModifiedDateTimeUtc)
               .IsRequired()
               .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedByUser)
                   .IsRequired()
                   .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder.Property(x => x.ModifiedByUser)
                   .IsRequired()
                   .HasColumnType("VARCHAR(64) COLLATE NOCASE");
        }
    }
}
