using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace suntvaccinat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OrganiserBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Organiser.EventsListPage());
        }
        private void ClientBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}
