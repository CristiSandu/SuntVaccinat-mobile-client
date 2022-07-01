using suntvaccinat.Models;
using suntvaccinat.Services.Interfaces;
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
                if (_isAnalyzing != value)
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
                if (_isScanning != value)
                {
                    _isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public bool IsLoading { get => !_isScanning; }
        public Result Result { get; set; }

        IDevice _getDeviceInfo;
        IEventsDataBase _database;
        Services.IValidationServiceAPI _validationServiceApi;

        private bool _used = false;
        public ICommand ScanCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Certificate = Result.Text;
                    if (_used)
                        return;

                    if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                    {
                        _used = true;
                        var phoneId = _getDeviceInfo.GetIdentifier();
                        User user = await _database.GetUser();

                        var valModelRespons = await Services.ValidationCertificate.GetValueToSaveOnServer(Certificate, phoneId, user);
                        if (valModelRespons == null)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Certificate is not valid", "Ok");
                            });
                            return;
                        }

                        var checkCertificate = await _validationServiceApi.ApiValidationCheckIfExistCertificateAsync(new Services.CheckIfCertificateExistRequest { CertificateId = valModelRespons.CertificateId, IsINSP = false });

                        bool responsPost;

                        if (checkCertificate)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.Current.MainPage.DisplayAlert("Duplicate Found", "This certificate is use with another phone", "OK");
                                await Application.Current.MainPage.Navigation.PopAsync();
                            });
                            return;
                        }

                        responsPost = await _validationServiceApi.ApiValidationPostAsync(valModelRespons);

                        await SecureStorage.SetAsync(Helpers.Constants.GreenPass, $"{Certificate}////{phoneId}");
                        Preferences.Set(Helpers.Constants.GreenPass, true);

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert(Helpers.Constants.SuccessMsg, "Green Pass Add Succesfully!", "Ok");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Certificate is not valid", "Ok");
                        });
                    }

                    IsAnalyzing = true;
                    IsScanning = true;
                });
            }
        }
        public ScanClientViewModel()
        {
            _getDeviceInfo = DependencyService.Get<IDevice>();
            _database = DependencyService.Get<IEventsDataBase>();
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();
        }
    }
}
