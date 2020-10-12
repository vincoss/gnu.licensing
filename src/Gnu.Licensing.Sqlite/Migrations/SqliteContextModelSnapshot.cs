﻿// <auto-generated />
using System;
using Gnu.Licensing.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gnu.Licensing.Sqlite.Migrations
{
    [DbContext(typeof(SqliteContext))]
    partial class SqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseActivation", b =>
                {
                    b.Property<int>("LicenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ActivationUuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("AttributesChecksum")
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("ChecksumType")
                        .IsRequired()
                        .HasColumnType("VARCHAR(12) COLLATE NOCASE");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64) COLLATE NOCASE");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("DATETIME");

                    b.Property<string>("LicenseAttributes")
                        .HasColumnType("NVARCHAR COLLATE NOCASE");

                    b.Property<string>("LicenseChecksum")
                        .IsRequired()
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("LicenseString")
                        .IsRequired()
                        .HasColumnType("NVARCHAR COLLATE NOCASE");

                    b.Property<Guid>("LicenseUuid")
                        .HasColumnType("VARCHAR(36)");

                    b.Property<Guid>("ProductUuid")
                        .HasColumnType("VARCHAR(36)");

                    b.HasKey("LicenseId");

                    b.HasIndex("LicenseId")
                        .IsUnique();

                    b.ToTable("LicenseActivation");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseCompany", b =>
                {
                    b.Property<int>("LicenseCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

                    b.Property<Guid>("CompanyUuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(36)")
                        .HasDefaultValue(new Guid("7040d84b-86b1-4ea9-9394-7b0b84fc784e"));

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64) COLLATE NOCASE");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasDefaultValue(new DateTime(2020, 10, 12, 22, 18, 56, 47, DateTimeKind.Utc).AddTicks(7343));

                    b.HasKey("LicenseCompanyId");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.HasIndex("CompanyUuid")
                        .IsUnique();

                    b.HasIndex("LicenseCompanyId", "CompanyName")
                        .IsUnique();

                    b.ToTable("LicenseCompany");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseProduct", b =>
                {
                    b.Property<int>("LicenseProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64) COLLATE NOCASE");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasDefaultValue(new DateTime(2020, 10, 12, 22, 18, 56, 49, DateTimeKind.Utc).AddTicks(7882));

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(1024) COLLATE NOCASE");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

                    b.Property<Guid>("ProductUuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(36)")
                        .HasDefaultValue(new Guid("d2b71c0f-df99-4ea5-ac42-6a624362c9dc"));

                    b.Property<string>("SignKeyName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(64) COLLATE NOCASE");

                    b.HasKey("LicenseProductId");

                    b.HasIndex("ProductName")
                        .IsUnique();

                    b.HasIndex("ProductUuid")
                        .IsUnique();

                    b.HasIndex("LicenseProductId", "ProductName")
                        .IsUnique();

                    b.ToTable("LicenseProduct");
                });

            modelBuilder.Entity("Gnu.Licensing.Core.Entities.LicenseRegistration", b =>
                {
                    b.Property<int>("LicenseRegistrationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("NVARCHAR(1024) COLLATE NOCASE");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64) COLLATE NOCASE");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasDefaultValue(new DateTime(2020, 10, 12, 22, 18, 56, 54, DateTimeKind.Utc).AddTicks(3302));

                    b.Property<DateTime?>("ExpireUtc")
                        .HasColumnType("DATETIME");

                    b.Property<bool>("IsActive")
                        .HasColumnType("BOOLEAN");

                    b.Property<string>("LicenseEmail")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

                    b.Property<string>("LicenseName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

                    b.Property<int>("LicenseType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LicenseUuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(36)")
                        .HasDefaultValue(new Guid("4232a2b4-1f51-4228-befd-cc02a6d8a093"));

                    b.Property<Guid>("ProductUuid")
                        .HasColumnType("VARCHAR(36)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.HasKey("LicenseRegistrationId");

                    b.HasIndex("LicenseUuid")
                        .IsUnique();

                    b.HasIndex("LicenseRegistrationId", "LicenseName", "LicenseEmail", "IsActive")
                        .IsUnique();

                    b.ToTable("LicenseRegistration");
                });
#pragma warning restore 612, 618
        }
    }
}
