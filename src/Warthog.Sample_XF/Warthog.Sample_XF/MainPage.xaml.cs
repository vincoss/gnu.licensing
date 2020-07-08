using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warthog.Sample_XamarinForms.Views;
using Xamarin.Forms;

namespace Warthog.Sample_XF
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
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new LicenseView());
        }
    }
}
