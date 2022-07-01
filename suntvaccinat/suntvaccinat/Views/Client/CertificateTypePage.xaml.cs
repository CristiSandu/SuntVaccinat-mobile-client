using suntvaccinat.Views.Client.GreenPass;
using suntvaccinat.Views.Client.INSP;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CertificateTypePage : ContentPage
    {
        public CertificateTypePage()
        {
            InitializeComponent();
        }

        private async void GreenPass_Flow_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get(Helpers.Constants.GreenPass, false))
                await Navigation.PushAsync(new ProfilePage());
            else
                await Navigation.PushAsync(new EnterGreenPassPage());
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void INSPBtn_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get(Helpers.Constants.INSPPref, false))
                await Navigation.PushAsync(new ProfilePage());
            else
                await Navigation.PushAsync(new EnterINSPPage());
        }
    }
}