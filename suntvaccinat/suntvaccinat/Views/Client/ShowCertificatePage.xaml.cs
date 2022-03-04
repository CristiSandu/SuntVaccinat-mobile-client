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
    public partial class ShowCertificatePage : ContentPage
    {
        public ShowCertificatePage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Client.ShowGreenPassViewModel();
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Warning", "Do you want to remove green pass ?", "YES", "NO");
            if (res)
            {
                SecureStorage.Remove(Helpers.Constants.GreenPass);
                Preferences.Remove(Helpers.Constants.GreenPass);
                await Navigation.PopAsync();
            }
        }

        private async void ADSButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync(Helpers.Constants.SiteURL, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
} 