using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace suntvaccinat.ViewModels.Client
{
    public class ShowGreenPassViewModel : BaseViewModel
    {
        public string GreenPass { get; set; }
        public string Title { get; set; } = "Green Pass";
        public ShowGreenPassViewModel()
        {
            GetGreenPass();
        }

        public async void GetGreenPass()
        {
            try
            {
                var greenPassValue = await SecureStorage.GetAsync(Helpers.Constants.GreenPass);
                GreenPass = greenPassValue;
                OnPropertyChanged(nameof(GreenPass));
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
    }
}
