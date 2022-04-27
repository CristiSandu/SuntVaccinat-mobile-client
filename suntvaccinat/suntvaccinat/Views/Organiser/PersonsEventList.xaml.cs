using suntvaccinat.Models;
using suntvaccinat.ViewModels.Organiser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Organiser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonsEventList : ContentPage
    {
        int _idEvent = 0;

        public PersonsEventList()
        {
            InitializeComponent();
        }

        public PersonsEventList(string eventName, int eventId)
        {
            InitializeComponent();
            _idEvent= eventId;
            BindingContext = new PersonsEventListViewModel(eventName, eventId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as PersonsEventListViewModel).GetPersons();
        }

        private async void ButtonAddPerson_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanPage { BindingContext = new ScanOrganiserViewModel(_idEvent) });
        }

        private async void ButtonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}