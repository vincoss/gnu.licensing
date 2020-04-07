using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shot.Licensing.Validation;
using System.Net.Http;
using Xamarin.Essentials;


namespace Shot.Licensing.Sample_XamarinForms.Services
{
    public class LicenseService : BaseLicenseService, ILicenseService
    {
        public LicenseService(HttpClient client) : base(client)
        {
        }

        protected override IEnumerable<IValidationFailure> ValidateInternal(License actual)
        {
            var failure = FailureStrings.Get(FailureStrings.ACT14Code);

            var appId = GetAttributes()[LicenseGlobals.AppIdKey];

            var failures = actual.Validate()
                                        .ExpirationDate()
                                        .When(lic => lic.Type == LicenseType.Standard)
                                        .And()
                                        .Signature(LicenseGlobals.PublicKey)
                                        .And()
                                        .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseGlobals.AppIdKey), StringComparison.OrdinalIgnoreCase), failure)
                                        .AssertValidLicense().ToList();

            return failures;
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
            var id = Preferences.Get(LicenseGlobals.AppIdKey, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException("Missing app ID");
            }

            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseGlobals.AppIdKey, id);
            return attributes;
        }

        public Task Run()
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
