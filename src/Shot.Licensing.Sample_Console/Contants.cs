using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shot.Licensing.Sample_Console
{
    public static class Contants
    {
        public const string LicenseServerUrl = "https://localhost:5001/api/license";

        public static string PublicKey = new StreamReader(typeof(Contants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_Console.Data.Public.xml")).ReadToEnd();
    }
}
