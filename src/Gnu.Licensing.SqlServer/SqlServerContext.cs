using Gnu.Licensing.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnu.Licensing.SqlServer
{
    public class SqlServerContext : AbstractContext<SqlServerContext>
    {
        /// <summary>
        /// The SQL Server error code for when a unique contraint is violated.
        /// </summary>
        private const int UniqueConstraintViolationErrorCode = 2627;

        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        { }

        public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
        {
            if (exception.GetBaseException() is SqlException sqlException)
            {
                return sqlException.Errors
                    .OfType<SqlError>()
                    .Any(error => error.Number == UniqueConstraintViolationErrorCode);
            }

            return false;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
