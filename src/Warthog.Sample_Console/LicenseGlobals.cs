using System.IO;


namespace Warthog.Sample_Console
{
    public static class LicenseGlobals
    {
        public const string AppId = "AppId";

        public static string PublicKey = new StreamReader(typeof(LicenseGlobals)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "Warthog.Sample_Console.Data.test.public.xml")).ReadToEnd();
    }
}
