using Shot.Licensing.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Shot.Licensing.Sample_Console_ServerLicenseFetch.Services
{
    public interface ILicenseService
    {
        Task<string> Register(string licenseRequest, IDictionary<string, string> attributes, string serverUrl);
    }
}
