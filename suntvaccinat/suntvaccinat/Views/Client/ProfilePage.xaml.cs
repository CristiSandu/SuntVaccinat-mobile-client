using suntvaccinat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Client.ProfilePageViewModel();

            //string deviceIdentifier = DependencyService.Get<IDevice>().GetIdentifier();
            //TestIMEI.Text = deviceIdentifier;
        }

        private async void GreenPassButton_Clicked(object sender, EventArgs e)
        {
            await TopLabel.TranslateTo(0, 0, 0, Easing.Linear);
            await MainCodeQR.ScaleTo(1, 250, Easing.CubicOut);
            await MainCodeQR.ScaleTo(1.05,250,Easing.CubicOut);
            await TopLabel.TranslateTo(0,-20,250, Easing.Linear);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}