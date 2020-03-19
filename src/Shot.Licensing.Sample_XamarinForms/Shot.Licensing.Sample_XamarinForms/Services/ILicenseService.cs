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


        Task<LicenseResult> RegisterAsync(Guid licenseKey, Guid productId);

        Task<LicenseResult> Validate();

        Task SetLicenseKeyAsync(string key);

        Task Run();
    }
}
