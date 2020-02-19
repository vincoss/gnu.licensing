using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Shot.Licensing.Sample_Console.Services
{
    public interface ILicenseService
    {
        Task<string> Register(string licenseRequest, IDictionary<string, string> attributes, string serverUrl);
    }
}
