using suntvaccinat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public bool IsShown { get; set; } = false;

        public string PhoneModel { get; set; }
        public ProfilePage()
        {
            InitializeComponent();
            var device = DeviceInfo.Model;
            var manufacturer = DeviceInfo.Manufacturer;
            PhoneModel = $"{manufacturer} - {device}";

            BindingContext = this;

            //string deviceIdentifier = DependencyService.Get<IDevice>().GetIdentifier();
            //TestIMEI.Text = deviceIdentifier;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IsShown = !IsShown;
            ButtonBigQr.IsVisible = IsShown;
            QRCode3.IsVisible = IsShown;
            GreenPassButton.IsVisible = !IsShown;
        }
    }
}