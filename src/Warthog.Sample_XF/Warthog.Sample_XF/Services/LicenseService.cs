using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Warthog.Validation;
using System.Net.Http;
using Xamarin.Essentials;


namespace Warthog.Sample_XamarinForms.Services
{
    public class LicenseService : BaseLicenseService, ILicenseService
    {
        public LicenseService(HttpClient client) : base(client)
        {
        }

        protected override Task<IEnumerable<IValidationFailure>> ValidateInternal(License actual)
        {
            var failure = FailureStrings.Get(FailureStrings.VAL04Code);

            var appId = GetAttributes()[LicenseGlobals.AppId];

            var failures = actual.Validate()
                                        .ExpirationDate()
                                        .When(lic => lic.Type == LicenseType.Standard)
                                        .And()
                                        .Signature(LicenseGlobals.PublicKey)
                                        .And()
                                        .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseGlobals.AppId), StringComparison.OrdinalIgnoreCase), failure)
                                        .AssertValidLicense().ToList();

            return Task.FromResult(failures.AsEnumerable());
        }

        protected override Stream LicenseOpenRead()
        {
            if(File.Exists(GetPath()))
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "license.xml");
        }

        protected virtual IDictionary<string, string> GetAttributes()
        {
            var id = Preferences.Get(LicenseGlobals.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException("Missing app ID");
            }

            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseGlobals.AppId, id);
            return attributes;
        }

        public Task RunAsync()
        {
            var task = Task.Run(async() =>
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
            return RegisterAsync(licenseKey, LicenseGlobals.ProductId, LicenseGlobals.LicenseServerUrl, GetAttributes());
        }
    }
}
