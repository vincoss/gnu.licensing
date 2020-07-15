using Gnu.Licensing.Sample_XamarinForms;
using Gnu.Licensing.Sample_XamarinForms.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gnu.Licensing.Sample_XF
{
    public partial class App : Application
    {
        public IApplicationContext ApplicationContext = new ApplicationContext();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Example: App first start create new app id and store it. Then use it with license attributes.
            ApplicationContext.GetAppId();

            // Run license service in the backgroud, will set Demo|Full version for the app
            new LicenseService(ApplicationContext, BaseLicenseService.CreateHttpClient()).RunAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
