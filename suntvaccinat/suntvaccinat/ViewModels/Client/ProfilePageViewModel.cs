using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Client
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public User User { get; set; }
        public string PhoneMode { get; set; }
        public string GPCertificate { get; set; } = string.Empty;
        public string INSPCertificate { get; set; } = string.Empty;
        public bool IsShown { get; set; } = false;

        public string SelectedCertificate { get; set; }

        Services.IEventsDataBase _eventsDataBase;

        public ICommand ShowCertCommand { get; set; }


        public ProfilePageViewModel()
        {
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();

            ShowCertCommand = new Command<string>((elem) =>
           {
               IsShown = !IsShown;
               if (elem != null)
                   SelectedCertificate = elem == "GP" ? GPCertificate : INSPCertificate;
           });
        }

        public async Task GetCertificatesAsync()
        {
            GPCertificate = await SecureStorage.GetAsync(Helpers.Constants.GreenPass);
            INSPCertificate = await SecureStorage.GetAsync(Helpers.Constants.INSPPref);

            OnPropertyChanged(nameof(GPCertificate));
            OnPropertyChanged(nameof(INSPCertificate));
        }
    }
}
