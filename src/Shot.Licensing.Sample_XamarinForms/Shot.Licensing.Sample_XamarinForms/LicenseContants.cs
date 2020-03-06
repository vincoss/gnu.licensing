using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace samplesl.Sample_XamarinForms
{
    // TODO:
    public static class LicenseContants
    {
        public const string AppId = "AppId";
        public const string LicenseServerUrl = "http://localhost:5000/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "samplesl.Sample_XamarinForms.Data.test.public.xml")).ReadToEnd();

    }
}
