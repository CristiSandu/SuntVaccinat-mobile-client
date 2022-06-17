using suntvaccinat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private Button CheckButtonTheme { get; set; }
        private Button CheckButtonLanguage { get; set; }

        public SettingsPage()
        {
            InitializeComponent();

            switch (Settings.Theme)
            {
                case 0:
                    DarkBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    CheckButtonTheme = DarkBtn;
                    break;
                case 1:
                    LightBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    CheckButtonTheme = LightBtn;
                    break;
                case 2:
                    DarkBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    CheckButtonTheme = DarkBtn;
                    break;
                default:
                    break;
            }

            switch (Settings.Language)
            {
                case "ro":
                    RoBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Language = "ro";
                    CheckButtonLanguage = RoBtn;
                    break;
                case "en":
                    EnBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Language = "en";
                    CheckButtonLanguage = EnBtn;
                    break;
                default:
                    EnBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Language = "en";
                    CheckButtonLanguage = EnBtn;
                    break;
            }
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void ChangeTheme_Clicked(object sender, EventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (CheckButtonTheme.Text == btn.Text)
                return;

            CheckButtonTheme.Style = (Style)Application.Current.Resources["ButtonCheckedStyle"];
            switch (btn.Text)
            {
                case "Light":
                    LightBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Theme = 1;
                    CheckButtonTheme = LightBtn;
                    break;
                case "Dark":
                    DarkBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Theme = 2;
                    CheckButtonTheme = DarkBtn;
                    break;
                default:
                    break;
            }

            Settings.SetTheme();
        }

        private async void ChangeLanguage_Clicked(object sender, EventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (CheckButtonLanguage.Text == btn.Text)
                return;

            CheckButtonLanguage.Style = (Style)Application.Current.Resources["ButtonCheckedStyle"];
            switch (btn.Text)
            {
                case "Ro":
                    RoBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Language = "ro";
                    CheckButtonLanguage = RoBtn;
                    break;
                case "En":
                    EnBtn.Style = (Style)Application.Current.Resources["ButtonUnCheckStyle"];
                    Settings.Language = "en";
                    CheckButtonLanguage = EnBtn;
                    break;
                default:
                    break;
            }

            await DisplayAlert("Alert", "For apply changes need to restart the app", "OK");

            Settings.SetLanguage();
        }
    }
}