using Shot.Licensing.Sample_Console.Services;
using Standard.Licensing;
using Standard.Licensing.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Shot.Licensing.Sample_Console
{
    class Program
    {
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
                1. Assume that the user already paid for the license and received an license ID. 
                2. Call license service to generate actual license.
                3. Pass additional parameters to lock the license to a particular machine or software installation. The passed parameters will be added into the license file.
                4. Save license into client device
                5. Validate lincese on the client device
            */

            var licenseId = "D4248D45-7B4A-4832-A7D1-6AA32A752453";
            const string DeviceId = "FAAAEB70-3BCF-4FDC-B67A-5C6B81C316C5"; // Device unique id. (example)

            var attributes = new Dictionary<string, string>();
            attributes.Add("ClientId", DeviceId);

            // Get a license file from the server.
            var service = new LicenseService();
            var licenseStr = await service.Register(licenseId, attributes, Contants.LicenseServerUrl);
            var directory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var licenseFile = Path.Combine(directory, $"license.xml");

            // License could not be generated, or server is not available
            if (string.IsNullOrWhiteSpace(licenseStr))
            {
                Console.WriteLine("Could not register a license");
                return;
            }

            // Got license so save it somehwere
            Save(licenseFile, licenseStr);

            // Check if valid
            License actual;
            using (var stream = File.OpenRead(licenseFile))
            {
                actual = License.Load(stream);
            }

            var failure = new LicenseExpiredValidationFailure()
            {
                Message = "License not valid for current deveice.",
                HowToResolve = "This is test only"
            };

            var validationFailures = actual.Validate()
                                          .ExpirationDate()
                                          .When(lic => lic.Type == LicenseType.Standard)
                                          .And()
                                          .Signature(Contants.PublicKey)
                                          .And()
                                          .AssertThat(x => x.AdditionalAttributes.Get("ClientId") == DeviceId, failure)
                                          .AssertValidLicense().ToList();

            if(validationFailures.Any())
            {
                foreach(var v in validationFailures)
                {
                    Console.WriteLine(v.Message);
                    Console.WriteLine(v.HowToResolve);
                }
            }
            else
            {
                Console.WriteLine("Device is licensed.");
            }
        }

        private static void Save(string path, string content)
        {
            using (var sw = new StreamWriter(path, false))
            {
                sw.Write(content);
            }
        }

    }
}