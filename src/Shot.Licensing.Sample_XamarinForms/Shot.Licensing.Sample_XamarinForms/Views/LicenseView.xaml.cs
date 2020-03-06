using samplesl.Sample_XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace samplesl.Sample_XamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LicenseView : ContentPage
    {
        public LicenseView()
        {
            InitializeComponent();

            var model = new LicenseViewModel();
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