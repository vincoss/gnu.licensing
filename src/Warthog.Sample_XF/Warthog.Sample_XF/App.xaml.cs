using System;
using Warthog.Sample_XamarinForms;
using Warthog.Sample_XamarinForms.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Warthog.Sample_XF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        { 
            // Example: App first start create new app id and store it.
            var id = Preferences.Get(LicenseGlobals.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                Preferences.Set(LicenseGlobals.AppId, Guid.NewGuid().ToString());
            }

            // Run license service in the backgroud, will set Demo|Full version for the app
            new LicenseService(BaseLicenseService.CreateHttpClient()).RunAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
