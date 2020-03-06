using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace samplesl.Test
{
    public class BaseLicenseTest
    {
        public static string PublicKey = new StreamReader(typeof(BaseLicenseTest)
                                                     .Assembly
                                                     .GetManifestResourceStream(
                                                     "samplesl.Test.Data.Pulic.xml")).ReadToEnd();

        public static string PrivateKey = new StreamReader(typeof(BaseLicenseTest)
                                                   .Assembly
                                                   .GetManifestResourceStream(
                                                   "samplesl.Test.Data.Private.xml")).ReadToEnd();

        public static DateTime ConvertToRfc1123(DateTime dateTime)
        {
            return DateTime.ParseExact(
                dateTime.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
                , "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public static void RobustDelete(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            var attempt = 0;
            start:
            try
            {
                if(File.Exists(path) == false)
                {
                    return;
                }
                attempt++;
                File.Delete(path);
            }
            catch
            {
                if(attempt > 10)
                {
                    throw;
                }
                Thread.Sleep(500);
                goto start;
            }
        }
    }
}