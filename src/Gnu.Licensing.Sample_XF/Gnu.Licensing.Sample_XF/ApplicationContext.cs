using System;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace Gnu.Licensing.Sample_XamarinForms
{
    public class ApplicationContext : IApplicationContext
    {
        public string GetAppId()
        {
            var id = Preferences.Get(LicenseGlobals.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                Preferences.Set(LicenseGlobals.AppId, Guid.NewGuid().ToString());
            }
            id = Preferences.Get(LicenseGlobals.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException("Missing app ID");
            }

            return id;
        }

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
