using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shot.Licensing.Sample_XamarinForms
{
    public static class LicenseContants
    {
        public const string AppId = "AppId";
        public const string LicenseServerUrl = "https://localhost:5001/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_XamarinForms.Data.test.public.xml")).ReadToEnd();

    }
}
