using suntvaccinat.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace suntvaccinat.Helpers
{
    public class Settings
    {
        const int theme = 0;
        const string language = "en";

        public static int Theme
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(Theme), theme);
            set => Xamarin.Essentials.Preferences.Set(nameof(Theme), value);
        }

        public static string Language
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(Language), language);
            set => Xamarin.Essentials.Preferences.Set(nameof(Language), value);
        }

        public static void SetTheme()
        {
            switch (Theme)
            {
                // default
                case 0:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Unspecified;
                    break;
                // light
                case 1:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Light;
                    break;
                // dark
                case 2:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Dark;
                    break;
            }
        }

        public static void SetLanguage()
        {
            switch (Language)
            {
                case "en":
                    AppResources.Culture = new CultureInfo(Language);
                    break;
                case "ro":
                    AppResources.Culture = new CultureInfo(Language);
                    break;
                default:
                    AppResources.Culture = CultureInfo.CurrentCulture;
                    break;
            }
        }
    }
}
