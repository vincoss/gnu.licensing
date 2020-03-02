using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shot.Licensing.Sample_Console
{
    public static class LicenseContants
    {
        public const string ClientVersion = "Version";
        public const string AppId = "AppId";
        public const string LicenseServerUrl = "https://localhost:5001/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_Console.Data.test.public.xml")).ReadToEnd();

    }
}
