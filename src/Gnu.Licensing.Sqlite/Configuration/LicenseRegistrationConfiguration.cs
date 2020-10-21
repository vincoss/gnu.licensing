using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gnu.Licensing.Sqlite.Configuration
{
    public class LicenseRegistrationConfiguration : IEntityTypeConfiguration<LicenseRegistration>
    {
        public void Configure(EntityTypeBuilder<LicenseRegistration> builder)
        {
            builder
              .Property(t => t.LicenseRegistrationId)
              .HasColumnType("VARCHAR(36)");

            builder
              .Property(t => t.ProductId)
              .HasColumnType("VARCHAR(36)");

            builder
           .Property(t => t.CompanyId)
           .HasColumnType("VARCHAR(36)");

            builder
            .Property(t => t.LicenseName)
            .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder
            .Property(t => t.LicenseEmail)
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder
                .Property(t => t.LicenseType)
                .HasColumnType("INTEGER");

            builder
           .Property(t => t.IsActive)
                .HasColumnType("BOOLEAN");

            builder
          .Property(t => t.Comment)
          .HasColumnType("NVARCHAR(1024) COLLATE NOCASE");

            builder
             .Property(t => t.Quantity)
             .HasColumnType("INTEGER");

            builder
          .Property(t => t.ExpireUtc)
          .HasColumnType("DATETIME");


            builder
              .Property(t => t.CreatedDateTimeUtc)
              .HasColumnType("DATETIME");

            builder
                .Property(t => t.CreatedByUser)
                .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
