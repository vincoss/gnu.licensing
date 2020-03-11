using samplesl.Sample_XamarinForms.Services;
using samplesl.Sample_XamarinForms.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace samplesl.Sample_XamarinForms
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
            var id = Preferences.Get(LicenseContants.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                Preferences.Set(LicenseContants.AppId, Guid.NewGuid().ToString());
            }

            new LicenseService().Run();

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
