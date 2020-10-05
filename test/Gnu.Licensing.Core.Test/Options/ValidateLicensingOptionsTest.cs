using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace Gnu.Licensing.Core.Options
{
    public class ValidateLicensingOptionsTest
    {
        [Fact]
        public void ValidTest()
        {
            var validator = new ValidateLicensingOptions<ApplicationOptions>(null);

            var options = new ApplicationOptions();

            var result = validator.Validate(null, options);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void ValidateDatabaseOptions_HasErrors()
        {
            var validator = new ValidateLicensingOptions<DatabaseOptions>(nameof(ApplicationOptions.Database));

            var options = new DatabaseOptions();

            var result = validator.Validate(null, options);

            Assert.True(result.Failed);
        }

        [Fact]
        public void ValidateDatabaseOptions_Valid()
        {
            var validator = new ValidateLicensingOptions<DatabaseOptions>(nameof(ApplicationOptions.Database));

            var options = new DatabaseOptions()
            {
                ConnectionString = "test-con",
                Type = "Sqlite"
            };

            var result = validator.Validate(null, options);

            Assert.True(result.Succeeded);
        }
    }
}
