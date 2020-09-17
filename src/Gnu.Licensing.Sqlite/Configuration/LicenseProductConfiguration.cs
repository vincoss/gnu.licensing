using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gnu.Licensing.Sqlite.Configuration
{
    public class LicenseProductConfiguration : IEntityTypeConfiguration<LicenseProduct>
    {
        public void Configure(EntityTypeBuilder<LicenseProduct> builder)
        {
            builder
            .Property(t => t.LicenseProductId)
            .HasColumnType("INTEGER");

            builder
                .Property(t => t.ProductUuid)
                .HasColumnType("VARCHAR(36)");

            builder
               .Property(t => t.CompanyId)
               .HasColumnType("INTEGER");

            builder
             .Property(t => t.ProductName)
             .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder
           .Property(t => t.ProductDescription)
           .HasColumnType("NVARCHAR(1024) COLLATE NOCASE");

            builder
            .Property(t => t.SignKeyName)
                  .HasColumnType("VARCHAR(64) COLLATE NOCASE");

            builder
               .Property(t => t.CreatedDateTimeUtc)
               .HasColumnType("DATETIME");

            builder
                .Property(t => t.CreatedByUser)
                .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
