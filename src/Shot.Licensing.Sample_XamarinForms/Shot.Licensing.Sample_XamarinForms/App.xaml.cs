using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace samplesl.Sample_XamarinForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            new Helper().Run();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
