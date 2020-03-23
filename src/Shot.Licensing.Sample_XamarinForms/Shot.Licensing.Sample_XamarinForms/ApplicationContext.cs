using System;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace samplesl.Sample_XamarinForms
{
    public class ApplicationContext : IApplicationContext
    {
        public string GetValueOrDefault(string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return Preferences.Get(key, defaultValue);
        }

        public Task SetLicenseKeyAsync(string key)
        {
            return SecureStorage.SetAsync(LicenseGlobals.LicenseKey, key);
        }
    }
}
