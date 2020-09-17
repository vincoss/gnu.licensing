using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gnu.Licensing.Core.Entities.Configuration
{
    public class LicenseActivationConfiguration : IEntityTypeConfiguration<LicenseActivation>
    {
        public void Configure(EntityTypeBuilder<LicenseActivation> builder)
        {
            builder.ToTable(nameof(LicenseActivation))
                   .HasIndex(x => new { x.LicenseId, x.IsActive }).IsUnique();

            builder.HasKey(x => x.LicenseId);

            builder.Property(t => t.LicenseId)
                .IsRequired();

            builder.Property(t => t.LicenseUuid)
                   .IsRequired();

            builder.Property(t => t.ProductUuid)
                .IsRequired();

            builder.Property(t => t.CompanyId)
               .IsRequired();

            builder.Property(t => t.LicenseString)
                   .IsRequired();

            builder.Property(t => t.LicenseAttributes);

            builder.Property(t => t.LicenseChecksum)
                .IsRequired();

            builder.Property(t => t.AttributesChecksum);

            builder.Property(t => t.ChecksumType)
              .IsRequired();

            builder.Property(t => t.IsActive)
                      .IsRequired()
                      .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired();

            builder.Property(x => x.ModifiedDateTimeUtc)
               .IsRequired();

            builder.Property(x => x.CreatedByUser)
                   .IsRequired();

            builder.Property(x => x.ModifiedByUser)
                   .IsRequired();
        }
    }
}
