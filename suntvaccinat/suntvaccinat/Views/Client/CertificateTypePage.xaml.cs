using suntvaccinat.Views.Client.GreenPass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CertificateTypePage : ContentPage
    {
        public CertificateTypePage()
        {
            InitializeComponent();
        }

        private async void GreenPass_Flow_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EnterGreenPassPage());
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}