﻿// <auto-generated />
using System;
using Gnu.Licensing.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gnu.Licensing.SqlServer.Migrations
{
    [DbContext(typeof(SqlServerContext))]
    partial class SqlServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseActivation", b =>
                {
                    b.Property<Guid>("LicenseActivationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttributesChecksum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChecksumType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LicenseAttributes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseChecksum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LicenseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LicenseString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LicenseActivationId");

                    b.HasIndex("LicenseActivationId")
                        .IsUnique();

                    b.ToTable("LicenseActivation");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseCompany", b =>
                {
                    b.Property<Guid>("LicenseCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2020, 10, 21, 4, 17, 17, 744, DateTimeKind.Utc).AddTicks(3796));

                    b.HasKey("LicenseCompanyId");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.HasIndex("LicenseCompanyId")
                        .IsUnique();

                    b.ToTable("LicenseCompany");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseProduct", b =>
                {
                    b.Property<Guid>("LicenseProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2020, 10, 21, 4, 17, 17, 750, DateTimeKind.Utc).AddTicks(6609));

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SignKeyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LicenseProductId");

                    b.HasIndex("LicenseProductId")
                        .IsUnique();

                    b.HasIndex("ProductName")
                        .IsUnique();

                    b.ToTable("LicenseProduct");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseRegistration", b =>
                {
                    b.Property<Guid>("LicenseRegistrationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2020, 10, 21, 4, 17, 17, 755, DateTimeKind.Utc).AddTicks(3714));

                    b.Property<DateTime?>("ExpireUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LicenseEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LicenseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LicenseType")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("LicenseRegistrationId");

                    b.HasIndex("LicenseRegistrationId", "LicenseName", "LicenseEmail", "IsActive")
                        .IsUnique();

                    b.ToTable("LicenseRegistration");
                });
#pragma warning restore 612, 618
        }
    }
}
