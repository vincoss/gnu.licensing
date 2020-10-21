using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gnu.Licensing.Sqlite.Configuration
{
    public class LicenseActivationConfiguration : IEntityTypeConfiguration<LicenseActivation>
    {
        public void Configure(EntityTypeBuilder<LicenseActivation> builder)
        {
            builder
              .Property(t => t.LicenseActivationId)
              .HasColumnType("VARCHAR(36)");
            
            builder
             .Property(t => t.LicenseId)
             .HasColumnType("VARCHAR(36)");

            builder
             .Property(t => t.ProductId)
             .HasColumnType("VARCHAR(36)");

            builder
               .Property(t => t.CompanyId)
               .HasColumnType("INTEGER");

            builder
                .Property(t => t.LicenseString)
                .HasColumnType("NVARCHAR COLLATE NOCASE");

            builder
                .Property(t => t.LicenseAttributes)
                .HasColumnType("NVARCHAR COLLATE NOCASE");

            builder
                .Property(t => t.LicenseChecksum)
                .HasColumnType("NVARCHAR");

            builder
                .Property(t => t.AttributesChecksum)
                .HasColumnType("NVARCHAR");

            builder
                .Property(t => t.ChecksumType)
                .HasColumnType("VARCHAR(12) COLLATE NOCASE");

            builder
                .Property(t => t.CreatedDateTimeUtc)
                .HasColumnType("DATETIME");

            builder
                .Property(t => t.CreatedByUser)
                .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
