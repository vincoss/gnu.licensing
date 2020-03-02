using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Shot.Licensing.Sample_XamarinForms.Interfaces
{
    public interface ILicenseService
    {
        Task<string> Check(string licenseRequest, string serverUrl);

        Task<string> Register(string licenseRequest, IDictionary<string, string> attributes, string serverUrl);
    }
}
