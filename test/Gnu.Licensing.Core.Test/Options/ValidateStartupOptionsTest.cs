using Gnu.Licensing.Core.Options;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Text;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Gnu.Licensing.Core.Options
{
    public class ValidateStartupOptionsTest
    {
        [Fact]
        public void ValidTest()
        {
            var root = Microsoft.Extensions.Options.Options.Create(new ApplicationOptions());
            var database = Microsoft.Extensions.Options.Options.Create(new DatabaseOptions());
            var logger = new LoggerFactory().CreateLogger<ValidateStartupOptions>();

            var options = new ValidateStartupOptions(root, database, logger);

            var result = options.Validate();

            Assert.True(result);
        }
    }
}
