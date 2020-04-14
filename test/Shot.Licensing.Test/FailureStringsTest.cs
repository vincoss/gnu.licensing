using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Shot.Licensing.Test
{
    public class FailureStringsTest
    {
        [Fact]
        public void GetFailureTest()
        {
           var actual = FailureStrings.Get(FailureStrings.ACT00Code);

            Assert.Equal("ACT.00", actual.Code);
            Assert.Equal("Activation failed.", actual.Message);
            Assert.Equal("Your product is having trouble connecting with License servers.", actual.HowToResolve);
        }

        [Fact]
        public void GetFailureThrowsIfKeyNotFoundTest()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = FailureStrings.Get("Fake");
            });
        }

        [Fact]
        public void AllKeysValid()
        {
            foreach(var key in FailureStrings.GetKeys().Where(x => x.EndsWith("Code")))
            {
                var actual = FailureStrings.Get(key);

                Assert.NotNull(actual);
            }
        }
    }
}
