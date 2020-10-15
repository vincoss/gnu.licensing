using Gnu.Licensing.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Gnu.Licensing.Sample_Console_ServerLicenseFetch.Services
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey);

        Task<LicenseResult> ValidateAsync(bool onlineCheck = false);

        Task RunAsync();
    }
}
