using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gnu.Licensing.Sqlite.Configuration
{
    public class LicenseCompanyConfiguration : IEntityTypeConfiguration<LicenseCompany>
    {
        public void Configure(EntityTypeBuilder<LicenseCompany> builder)
        {
            builder.Property(t => t.LicenseCompanyId)
                   .HasColumnType("INTEGER");

            builder.Property(t => t.CompanyUuid)
                .HasColumnType("VARCHAR(36)");

            builder.Property(t => t.CompanyName)
                .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder.Property(t => t.CreatedDateTimeUtc)
                .HasColumnType("DATETIME");

            builder.Property(t => t.CreatedByUser)
                .HasColumnType("NVARCHAR(64) COLLATE NOCASE");
        }
    }
}
