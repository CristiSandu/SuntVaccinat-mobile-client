using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client.GreenPass
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterGreenPassPage : ContentPage
    {
        public EnterGreenPassPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Client.EnterCodeViewModel();
        }

        private async void ScannGreenPassBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanPage { BindingContext = new ViewModels.Client.ScanClientViewModel() });
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}