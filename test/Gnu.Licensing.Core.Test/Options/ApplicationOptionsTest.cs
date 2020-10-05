using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Gnu.Licensing.Core.Options
{
    public class ApplicationOptionsTest
    {
        [Fact]
        public void Test()
        {
            var options = new ApplicationOptions();

            Assert.False(options.UseCustomizationData);
            Assert.False(options.RunMigrationsAtStartup);
        }
    }
}
