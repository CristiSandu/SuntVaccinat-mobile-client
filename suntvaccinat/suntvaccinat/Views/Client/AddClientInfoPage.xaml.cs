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
        Models.User user = new Models.User(); 

        public AddClientInfoPage()
        {
            InitializeComponent();
            BindingContext = user;
        }

        private async void buttonAddPersonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void nextBtn_Clicked(object sender, EventArgs e)
        {

        }

        private async void cancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}