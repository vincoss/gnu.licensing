using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Xml.Linq;
using samplesl.Sample_Console_ServerLicenseFetch.Services;
using samplesl.Validation;


namespace samplesl.Sample_Console_ServerLicenseFetch
{
    class Program
    {
        static readonly string MachineName = Environment.MachineName;

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
                1. Assume that the user already paid for the license and received an license ID (GUID|UUID). 
                2. Call license service to generate actual license.
                3. Pass additional parameters to lock the license to a particular machine or software installation. The passed parameters will be added into the license file.
                4. Save license into client device
                5. Validate lincese on the client device
            */

            // User got license from purchase or regisration.
            var licenseId = "6F32D1AF-1E09-4D3C-87AE-0D5C22EB96C8";

            // Machine attributes to be embedded into the license.
            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseContants.MachineName, MachineName);

            // Get a license file from the server.
            var service = new LicenseService();
            var licenseStr = await service.Register(licenseId, attributes, LicenseContants.LicenseServerUrl);
            var directory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var licensePath = Path.Combine(directory, $"license.xml");

            // License could not be generated, or server is not available
            if (string.IsNullOrWhiteSpace(licenseStr))
            {
                Console.WriteLine("Could not register a license");
                return;
            }

           // Got license so save it somehwere
            var element = XElement.Parse(licenseStr);
            element.Save(licensePath);

            // Validate license
            using (var publicKey = new MemoryStream(Encoding.UTF8.GetBytes(LicenseContants.PublicKey)))
            using (var license = File.OpenRead(licensePath))
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
                                                   .AssertThat(x => string.Equals(MachineName, x.AdditionalAttributes.Get(LicenseContants.MachineName), StringComparison.OrdinalIgnoreCase), failure)
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
