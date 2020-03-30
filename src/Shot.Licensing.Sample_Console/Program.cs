using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shot.Licensing;
using Shot.Licensing.Validation;


namespace Shot.Licensing.Sample_Console
{
    class Program
    {
        const string AppId = "B3BCC7E4-6FC8-4CD7-A640-F50B1E5FC95B"; // This one is to lock the license to particular machine or software installation.

        static void Main(string[] args)
        {
            var p = new Program();
            p.Sample();

            Console.WriteLine("Done...");
            Console.Read();
        }

        public async void Sample()
        {
            /* 
                Validate license on the client device|machine
            */

            var directory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var licensePath = Path.Combine(directory, "data", $"license.xml");
            
            using(var publicKey = new MemoryStream(Encoding.UTF8.GetBytes(LicenseContants.PublicKey)))
            using(var license = File.OpenRead(licensePath))
            {
                var results = await Validate(license, publicKey);

                if (results.Any())
                {
                    foreach (var r in results)
                    {
                        Console.WriteLine(r.Message);
                        Console.WriteLine(r.HowToResolve);
                    }
                }
                else
                {
                    Console.WriteLine("Device is licensed.");
                }
            }
        }

        public Task<IEnumerable<IValidationFailure>> Validate(Stream license, Stream publicKey)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
            }
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            var task = Task.Run(() =>
            {
                var results = new List<IValidationFailure>();

                try
                {
                    var actual = License.Load(license);

                    var failure = FailureStrings.Get(FailureStrings.ACT14Code);

                    var validationFailures = actual.Validate()
                                                   .ExpirationDate()
                                                   .When(lic => lic.Type == LicenseType.Standard)
                                                   .And()
                                                   .Signature(LicenseContants.PublicKey)
                                                   .And()
                                                   .AssertThat(x => string.Equals(AppId, x.AdditionalAttributes.Get(LicenseContants.AppId), StringComparison.OrdinalIgnoreCase), failure)
                                                   .AssertValidLicense().ToList();

                    foreach (var f in validationFailures)
                    {
                        results.Add(f);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace with logger
                    Console.WriteLine(ex);

                    var failure = FailureStrings.Get(FailureStrings.ACT09Code);

                    results.Add(failure);
                }
                return results.AsEnumerable();
            });
            return task;
        }

    }
    
}