using System;
using System.Threading.Tasks;


namespace Warthog
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey);

        Task<LicenseResult> ValidateAsync();

        Task RunAsync();
    }
}
