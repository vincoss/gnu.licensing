using Gnu.Licensing.Sample_XamarinForms;
using Gnu.Licensing.Sample_XamarinForms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gnu.Licensing.Sample_XF
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            eLicUrl.Text = LicenseGlobals.LicenseServerUrl;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new LicenseView());
        }
    }
}
