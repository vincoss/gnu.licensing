using samplesl.Sample_XamarinForms.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace samplesl.Sample_XamarinForms.Interfaces
{
    public interface ILicenseService
    {
        Task<bool> HasConnection(string serverUrl);

        Task<CheckResult> Check(Guid licenseKey, Guid productId, string licenseSha256, string serverUrl);

        Task<RegisterResult> Register(Guid licenseKey, Guid productId, IDictionary<string, string> attributes, string serverUrl);
    }
}
