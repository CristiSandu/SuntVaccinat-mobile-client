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
        private string barcode = string.Empty;

        public string Barcode
        {
            get
            {
                return barcode;
            }
            set
            {
                barcode = value;
                OnPropertyChanged(nameof(Barcode));
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
                        Barcode = Result.Text;
                        await SecureStorage.SetAsync(Helpers.Constants.GreenPass, Barcode);
                        await Application.Current.MainPage.Navigation.PopAsync();
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
