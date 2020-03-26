using samplesl.Svr.Interface;
using samplesl.Svr.Models;
using samplesl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace samplesl.Svr.Services
{
    public class LicenseService : ILicenseService
    {
        public static string PrivateKey;
        static LicenseService()
        {
            PrivateKey = File.ReadAllText(@"C:\_Dev\GitHub\FL\shot.licensing\resources\test.private.xml"); // TODO:
        }

        public Task<string> Create(LicenseRegisterRequest register)
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
