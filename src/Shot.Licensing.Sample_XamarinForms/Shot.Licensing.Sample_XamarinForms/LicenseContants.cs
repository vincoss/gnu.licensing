using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace samplesl.Sample_XamarinForms
{
    // TODO:
    public static class LicenseContants
    {
        public static Guid ProductId = new Guid("6E7CCB7E-4940-4EAB-9D3A-FF48E72A0CA9");
        public const string AppId = "AppId";
        public const string LicenseServerUrl = "http://localhost:5000/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "samplesl.Sample_XamarinForms.Data.test.public.xml")).ReadToEnd();

    }
}
