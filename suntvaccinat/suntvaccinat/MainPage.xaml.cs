using suntvaccinat.Views;
using suntvaccinat.Views.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OrganiserBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Organiser.EventsListPage());
        }
        private async void ClientBtn_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get(Helpers.Constants.User, false))
            {
                if (Preferences.Get(Helpers.Constants.GreenPass, false) && Preferences.Get(Helpers.Constants.INSPPref, false))
                    await Navigation.PushAsync(new ProfilePage());
                else
                    await Navigation.PushAsync(new CertificateTypePage());
            }
            else
                await Navigation.PushAsync(new AddClientInfoPage());
        }

        private async void SettingsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsPage());
        }
    }
}
