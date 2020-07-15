using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gnu.Licensing.Sample_XamarinForms
{
    public interface IApplicationContext
    {
        string GetValueOrDefault(string key, string defaultValue);
        Task SetLicenseKeyAsync(string key);
        string GetAppId();
    }
}
