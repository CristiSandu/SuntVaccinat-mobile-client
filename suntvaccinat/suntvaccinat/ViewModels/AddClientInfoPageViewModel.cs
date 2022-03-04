using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels
{
    public class AddClientInfoPageViewModel : BaseViewModel
    {
        public User User { get; set; } = new User();
        public ICommand NextCommand { get; set; }
        public AddClientInfoPageViewModel()
        {
            NextCommand = new Command(async () =>
            {
                if (ValidationInput(User))
                {
                    SecureStorage.RemoveAll();
                    await SecureStorage.SetAsync(Helpers.Constants.User, User.ToString());
                    Preferences.Set(Helpers.Constants.User, true);

                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.SuccessMsg, "User Saved", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new Views.Client.CertificateTypePage());
                    return;
                }

                await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, "Same Fileds are empty", "Ok");
            });
        }

        public bool ValidationInput(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
                return false;

            if (string.IsNullOrEmpty(user.SecondName))
                return false;

            if (string.IsNullOrEmpty(user.Sex))
                return false;

            if (string.IsNullOrEmpty(user.Age) || !(Convert.ToInt32( user.Age) < 120 && Convert.ToInt32(user.Age) >= 1))
                return false;

            if (string.IsNullOrEmpty(user.PhoneNumber) || !Regex.Match(user.PhoneNumber, "^07[0-9]{8}$").Success)
                return false;

            return true;
        }
    }
}
