using System;
using System.IO;


namespace samplesl.Sample_XamarinForms
{
    public enum AppLicense : byte
    {
        Demo = 0,
        Full = 1
    }

    public static class LicenseContants
    {
        private static AppLicense _appLicense;
        public static Guid ProductId = new Guid("6E7CCB7E-4940-4EAB-9D3A-FF48E72A0CA9"); // Example
        public const string AppId = "AppId";
        public const string LicenseKey = "LicenseKey";
        public const string LicenseServerUrl = "http://localhost:5000/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_XamarinForms.Data.test.public.xml")).ReadToEnd();

        public static AppLicense Get()
        {
            return _appLicense;
        }

        internal static void Set(AppLicense type)
        {
            _appLicense = type;
        }
    }
}
