using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace suntvaccinat.ViewModels.Client
{
    public class ScanClientViewModel : BaseViewModel
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
        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set
            {
                if (!Equals(_isAnalyzing, value))
                {
                    _isAnalyzing = value;
                    OnPropertyChanged(nameof(IsAnalyzing));

                }
            }
        }

        private bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (!Equals(_isScanning, value))
                {
                    _isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                }
            }
        }
        public Result Result { get; set; }

        Services.IDevice _getDeviceInfo;
        Services.IEventsDataBase _database;
        Services.IValidationServiceAPI _validationServiceApi;

        private bool _used = false;
        public ICommand ScanCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Certificate = Result.Text;
                        if (_used)
                            return;

                        if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                        {
                            _used = true;
                            var phoneId = _getDeviceInfo.GetIdentifier();
                            User user = await _database.GetUser();

                            var valModelRespons = await Services.ValidationCertificate.GetValueToSaveOnServer(Certificate, phoneId, user);
                            var checkCertificate = await _validationServiceApi.ApiValidationCheckIfExistCertificateAsync(valModelRespons);

                            bool responsPost;

                            if (checkCertificate)
                            {
                                await App.Current.MainPage.DisplayAlert("Duplicate Found", "This certificate is use with another phone", "OK");
                                return;
                            }

                            responsPost = await _validationServiceApi.ApiValidationPostAsync(valModelRespons);

                            await SecureStorage.SetAsync(Helpers.Constants.GreenPass, $"{Certificate}////{phoneId}");
                            Preferences.Set(Helpers.Constants.GreenPass, true);

                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Certificate is not valid", "Ok");
                        }
                    });

                    IsAnalyzing = true;
                    IsScanning = true;
                });
            }
        }
        public ScanClientViewModel()
        {
            _getDeviceInfo = DependencyService.Get<Services.IDevice>();
            _database = DependencyService.Get<Services.IEventsDataBase>();
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();
        }
    }
}
