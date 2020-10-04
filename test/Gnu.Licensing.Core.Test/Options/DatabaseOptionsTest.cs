using Xunit;


namespace Gnu.Licensing.Core.Options
{
    public class DatabaseOptionsTest
    {
        [Fact]
        public void Test()
        {
            Assert.Equal("Sqlite", DatabaseOptions.Sqlite);
            Assert.Equal("SqlServer", DatabaseOptions.SqlServer);
        }
    }
}
