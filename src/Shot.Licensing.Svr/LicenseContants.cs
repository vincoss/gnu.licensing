using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace samplesl.Sample_Console_ServerLicenseFetch
{
    public static class LicenseContants
    {
        public const string MachineName = "MachineName";
        public const string LicenseServerUrl = "https://localhost:5001/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "samplesl.Sample_Console_ServerLicenseFetch.Data.test.public.xml")).ReadToEnd();

    }
}
