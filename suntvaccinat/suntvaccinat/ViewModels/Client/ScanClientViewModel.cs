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
                        if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                        {
                            await SecureStorage.SetAsync(Helpers.Constants.GreenPass, Certificate);
                            Preferences.Set(Helpers.Constants.GreenPass, true);
                            var result = await Services.ValidationCertificate.DecodeVerifyGreenPass(Certificate);
                            int i = 0;

                            await Application.Current.MainPage.Navigation.PopAsync();
                        }else
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

        }
    }
}
