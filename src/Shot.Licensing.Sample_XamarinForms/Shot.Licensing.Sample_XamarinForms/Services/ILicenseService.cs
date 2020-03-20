using samplesl.Sample_XamarinForms.Services;
using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace samplesl
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey, Guid productId, string url, IDictionary<string, string> attributes);

        Task<LicenseResult> ValidateAsync();

        Task SetLicenseKeyAsync(string key);

        Task Run();
    }
}
