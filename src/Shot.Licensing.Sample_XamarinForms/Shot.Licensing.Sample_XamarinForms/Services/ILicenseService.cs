using System;
using System.Threading.Tasks;


namespace samplesl
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey);

        Task<LicenseResult> ValidateAsync();

        Task SetLicenseKeyAsync(string key);

        Task Run();
    }
}
