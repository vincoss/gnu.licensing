using System;
using System.IO;


namespace Shot.Licensing.Sample_XamarinForms
{
    public enum AppLicense : byte
    {
        Demo = 0,
        Full = 1
    }

    public static class LicenseGlobals
    {
        private static AppLicense _appLicense;
        public static Guid ProductId = new Guid("C3F80BD7-9618-48F6-8250-65D113F9AED2"); // Example
        public const string AppIdKey = "AppId";
        public const string LicenseKey = "LicenseKey";
        public const string LicenseServerUrl = "http://localhost:5000/api/license";

        public static string PublicKey = new StreamReader(typeof(LicenseGlobals)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_XF.Data.test.public.xml")).ReadToEnd();

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
