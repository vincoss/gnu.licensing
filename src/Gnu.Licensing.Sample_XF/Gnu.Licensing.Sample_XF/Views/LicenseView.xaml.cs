using Gnu.Licensing.Sample_XamarinForms.Services;
using Gnu.Licensing.Sample_XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gnu.Licensing.Sample_XamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LicenseView : ContentPage
    {
        public LicenseView()
        {
            InitializeComponent();

            var service = new LicenseService(LicenseService.CreateHttpClient());
            var ctx = new ApplicationContext();

            var model = new LicenseViewModel(service, ctx);
            BindingContext = model;
        }

        protected override void OnAppearing()
        {
            var model = BindingContext as BaseViewModel;
            if(model != null)
            {
                model.Initialize();
            }

            base.OnAppearing();
        }
    }
}