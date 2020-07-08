using Gnu.Licensing;
using System;
using System.Collections.Generic;
using System.IO;


namespace Gnu.Licensing.Cli
{
    public class LicenseArgs
    {
        public FileInfo Sign { get; set; }
        public DirectoryInfo Directory { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public LicenseType Type { get; set; }
        public DateTime Expire { get; set; }
        public int Volume { get; set; }
        public IEnumerable<string> Features { get; set; }
        public IEnumerable<string> Additional { get; set; }
    }
}
