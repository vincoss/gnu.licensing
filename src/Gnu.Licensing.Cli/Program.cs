﻿using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Cli
{
    /// <example>
    /// dotnet Gnu.Licensing.Cli.dll keys -?
    /// dotnet Gnu.Licensing.Cli.dll keys -s 16384 -d c:\temp\lic
    /// dotnet Gnu.Licensing.Cli.dll license -?
    /// dotnet Gnu.Licensing.Cli.dll license -n name -e asd@gmail.com -t standard -x 2020-03-01 -v 2 -s "c:\temp\lic\20200228110715.private.xml" -d c:\temp\lic
    /// dotnet Gnu.Licensing.Cli.dll license -n name -e asd@gmail.com -t standard -x 2020-03-01 -v 2 -f "admin=true" -f "f1=v1" -a "version=1.0.0.0" -s "c:\temp\lic\20200228110715.private.xml" -d c:\temp\lic
    /// </example>
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await new ArgsService().Run(args);
        }
    }
}