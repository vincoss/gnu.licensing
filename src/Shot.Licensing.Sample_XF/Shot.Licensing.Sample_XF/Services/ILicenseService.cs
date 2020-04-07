using System;
using System.Threading.Tasks;


namespace Shot.Licensing
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey);

        Task<LicenseResult> ValidateAsync();

        Task Run();
    }
}
