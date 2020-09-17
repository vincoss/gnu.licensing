using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Sqlite.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Gnu.Licensing.Sqlite
{
    public class SqliteContext : AbstractContext<SqliteContext>
    {
        /// <summary>
        /// The Sqlite error code for when a unique constraint is violated.
        /// </summary>
        private const int SqliteUniqueConstraintViolationErrorCode = 19;

        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        { }

        public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
        {
            return exception.InnerException is SqliteException sqliteException &&
                sqliteException.SqliteErrorCode == SqliteUniqueConstraintViolationErrorCode;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LicenseCompanyConfiguration());
            builder.ApplyConfiguration(new LicenseProductConfiguration());
            builder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            builder.ApplyConfiguration(new LicenseActivationConfiguration());
        }
    }
}
