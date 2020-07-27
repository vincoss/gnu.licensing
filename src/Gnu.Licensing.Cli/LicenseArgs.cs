using Gnu.Licensing;
using System;
using System.Collections.Generic;
using System.IO;


namespace Gnu.Licensing.Cli
{
    public class LicenseArgs
    {
        public Guid LicenseId { get; set; }
        public IEnumerable<string> Additional { get; set; }
        public DirectoryInfo Directory { get; set; }
    }
}
