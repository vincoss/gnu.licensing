using Gnu.Licensing.Validation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.IO;
using System.Threading.Tasks;


namespace Gnu.Licensing.Sample_Console_ServerLicenseFetch.Services
{
    public class LicenseService : BaseLicenseService, ILicenseService
    {
        public LicenseService(HttpClient client) : base(client)
        {
        }

        protected override Task<IEnumerable<IValidationFailure>> ValidateInternalAsync(License actual)
        {
            var failure = FailureStrings.Get(FailureStrings.VAL04Code);

            var appId = GetAttributes()[LicenseGlobals.MachineName];

            var failures = actual.Validate()
                                        .ExpirationDate()
                                        .When(lic => lic.Type == LicenseType.Standard)
                                        .And()
                                        .Signature()
                                        .And()
                                        .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseGlobals.MachineName), StringComparison.OrdinalIgnoreCase), failure)
                                        .AssertValidLicense().ToList();

            return Task.FromResult(failures.AsEnumerable());
        }

        protected override Stream LicenseOpenRead()
        {
            if (File.Exists(GetPath()))
            {
                return File.OpenRead(GetPath());
            }
            return null;
        }

        protected override Stream LicenseOpenWrite()
        {
            return File.Open(GetPath(), FileMode.Create);
        }

        public string GetPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "license.xml");
        }

        protected virtual IDictionary<string, string> GetAttributes()
        {
            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseGlobals.MachineName, Environment.MachineName);
            return attributes;
        }

        public Task RunAsync()
        {
            var task = Task.Run(async () =>
            {
                var result = await ValidateAsync();
                if (result.Successful)
                {
                    LicenseGlobals.Set(AppLicense.Full);
                }
            });

            return task;
        }

        public Task<LicenseResult> RegisterAsync(Guid licenseKey)
        {
            return RegisterAsync(licenseKey, LicenseGlobals.ProductId, GetAttributes());
        }

        protected override bool IsConnected()
        {
            return true;
        }

        public override string GetLicenseServerUrl()
        {
            return "https://localhost/api/license";
        }
    }
}
