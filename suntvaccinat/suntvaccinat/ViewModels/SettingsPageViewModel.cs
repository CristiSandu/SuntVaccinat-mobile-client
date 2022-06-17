using suntvaccinat.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels
{
    public class SettingsPageViewModel : ObservableObject
    {
        List<(Func<string> name, string value)> languageMapping { get; } = new()
        {
            (() => AppResources.AgeLabel, null),
            (() => AppResources.CancelBtnLabel, "en"),
            (() => AppResources.CertificateMsgSecondPart, "ro")
        };

        public string CurrentLanguage { get; }


        public ICommand ChangeLanguageCommand { get; }

        public SettingsPageViewModel()
        {
            CurrentLanguage = GetCurrentLanguageName();
            //CurrentLanguage = new(() => LocalizationResourceManager.Current.CurrentCulture.DisplayName);

            ChangeLanguageCommand = new AsyncCommand(ChangeLanguage);
        }

        private string GetCurrentLanguageName()
        {
            var (knownName, _) = languageMapping.SingleOrDefault(m => m.value == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
            return knownName != null ? knownName() : LocalizationResourceManager.Current.CurrentCulture.DisplayName;
        }

        async Task ChangeLanguage()
        {
            string selectedName = await Application.Current.MainPage.DisplayActionSheet(
                "ChangeLanguage",
                null, null,
                languageMapping.Select(m => m.name()).ToArray());
            if (selectedName == null)
            {
                return;
            }

            string selectedValue = languageMapping.Single(m => m.name() == selectedName).value;

            LocalizationResourceManager.Current.SetCulture(selectedValue == null ? CultureInfo.CurrentCulture : new CultureInfo(selectedValue));
        }
    }
}
