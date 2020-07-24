using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Gnu.Licensing.Test
{
    public class BaseLicenseTest
    {
        public const string CertificateSearch = "CN=Gnu.Licensing";

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

        public void SetupCertificate()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var pfx = Path.Combine(path, "Data", "Gnu.Licensing.pfx.txt");

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadWrite))
            {
                store.Add(new X509Certificate2(pfx, "Pass@word1", X509KeyStorageFlags.PersistKeySet));
            }
        }
    }
}