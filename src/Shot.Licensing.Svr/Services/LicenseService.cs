using Shot.Licensing.Svr.Interface;
using Shot.Licensing.Svr.Models;
using Shot.Licensing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shot.Licensing.Svr.Services
{
    public class LicenseService : ILicenseService
    {
        public static string PrivateKey;
        static LicenseService()
        {
            PrivateKey = File.ReadAllText(@"C:\_Data\GitHub\Shot.Licensing\resources\test.private.xml"); // TODO:
        }

        public Task<string> Create(RegisterLicense register)
        {
            if(register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }
            var task = Task.Run(() =>
            {
                var license = License.New()
                     .WithUniqueIdentifier(register.LicenseId)
                     .As(LicenseType.Standard)
                     .ExpiresAt(DateTime.MaxValue)
                     .WithMaximumUtilization(1)
                     .LicensedTo("todo:", "todo@example.com")
                     .WithAdditionalAttributes(register.Attributes != null ? register.Attributes : new Dictionary<string, string>())
                     .CreateAndSignWithPrivateKey(PrivateKey);

                return license.ToString();
            });

            return task;
        }

    }
}
