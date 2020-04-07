using System.IO;


namespace Shot.Licensing.Sample_Console
{
    public static class LicenseGlobals
    {
        public const string AppId = "AppId";

        public static string PublicKey = new StreamReader(typeof(LicenseGlobals)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Shot.Licensing.Sample_Console.Data.test.public.xml")).ReadToEnd();
    }
}
