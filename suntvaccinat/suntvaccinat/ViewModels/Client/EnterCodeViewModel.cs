using suntvaccinat.Models;
using suntvaccinat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Client
{
    public class EnterCodeViewModel : BaseViewModel
    {
        private string _certificate = string.Empty;

        public string Certificate
        {
            get
            {
                return _certificate;
            }
            set
            {
                _certificate = value;
                OnPropertyChanged(nameof(Certificate));
            }
        }

        public ICommand SaveCertificateCommand { get; set; }
        public IDevice _getDeviceInfo;
        public IEventsDataBase _database;
        public Services.IValidationServiceAPI _validationServiceApi;

        private bool _used = false;

        public EnterCodeViewModel()
        {
            _getDeviceInfo = DependencyService.Get<IDevice>();
            _database = DependencyService.Get<IEventsDataBase>();
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();

            SaveCertificateCommand = new Command(async () =>
            {
                if (_used)
                    return;
                if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                {
                    _used = true;

                    var phoneId = _getDeviceInfo.GetIdentifier();
                    User user = await _database.GetUser();

                    var valModelRespons = await Services.ValidationCertificate.GetValueToSaveOnServer(Certificate, phoneId, user);
                    var responsTest = await _validationServiceApi.ApiValidationPostAsync(valModelRespons);

                    await SecureStorage.SetAsync(Helpers.Constants.GreenPass, $"{Certificate}////{phoneId}");
                    Preferences.Set(Helpers.Constants.GreenPass, true);

                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.SuccessMsg, "Certificate saved", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Certificate is not valid", "Ok");
                }
            });
        }
    }
}
