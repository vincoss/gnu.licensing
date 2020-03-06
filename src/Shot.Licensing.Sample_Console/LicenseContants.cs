using System.IO;


namespace samplesl.Sample_Console
{
    public static class LicenseContants
    {
        public const string AppId = "AppId";

        public static string PublicKey = new StreamReader(typeof(LicenseContants)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "samplesl.Sample_Console.Data.test.public.xml")).ReadToEnd();
    }
}
