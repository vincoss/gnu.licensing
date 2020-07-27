using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Cli
{
    /// <example>
    /// dotnet Gnu.Licensing.Cli.dll license -?
    /// dotnet Gnu.Licensing.Cli.dll license -? license
    /// dotnet Gnu.Licensing.Cli.dll license -n yourname -e youremail -t standard -x 2030-03-01 -v 2 -s "CN=Gnu.Licensing" -d c:\temp\lic
    /// dotnet Gnu.Licensing.Cli.dll license -n yourname -e youremail -t standard -x 2030-03-01 -v 2 -f "admin=true" -f "f1=v1" -a "version=1.0.0.0" -s "CN=Gnu.Licensing" -d c:\temp\lic
    /// </example>
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await new ArgsService().Run(args);
        }
    }

}