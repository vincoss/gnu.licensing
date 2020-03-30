using System.IO;


namespace Shot.Licensing.Sample_Console
{
    public static class LicenseContants
    {
        public const string AppId = "AppId";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_Console.Data.test.public.xml")).ReadToEnd();
    }
}
