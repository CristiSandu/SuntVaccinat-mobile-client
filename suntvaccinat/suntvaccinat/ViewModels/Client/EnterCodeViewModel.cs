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

        public EnterCodeViewModel()
        {
            SaveCertificateCommand = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                {
                    await SecureStorage.SetAsync(Helpers.Constants.GreenPass, Certificate);
                    Preferences.Set(Helpers.Constants.GreenPass, true);

                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.SuccessMsg, "Certificate saved", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }else
                {
                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Certificate is not valid", "Ok");
                }
            });
        }
    }
}
