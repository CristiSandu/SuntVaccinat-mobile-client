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
    public partial class AddClientInfoPage : ContentPage
    {
        public AddClientInfoPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.AddClientInfoPageViewModel();
        }

        private async void ButtonAddPersonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}