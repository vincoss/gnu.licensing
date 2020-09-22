using Gnu.Licensing.Api.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Services
{
    public class CertificateService : ICertificateService
    {
        public void Install(string path, string password, string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(thumbprint))
            {
                throw new ArgumentNullException(nameof(thumbprint));
            }
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            using (var certificate = new X509Certificate2(path, password, X509KeyStorageFlags.DefaultKeySet | X509KeyStorageFlags.PersistKeySet))
            {
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);

                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certificates.Count <= 0)
                {
                    throw new InvalidOperationException($"Unable to validate certificate was added to store. Thumbprint: {thumbprint}");
                }

                store.Close();
            }
        }
    }
}
