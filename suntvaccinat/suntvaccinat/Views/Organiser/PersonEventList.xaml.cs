using suntvaccinat.Models;
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
    public partial class PersonEventList : ContentPage
    {
        ObservableCollection<ParticipantModel> _parts = new ObservableCollection<ParticipantModel>();

        string _evName = string.Empty;
        int _idEv = 0;
        EventModel ev;

        Services.IEventsDataBase _eventsDataBase;
        public PersonEventList()
        {
            InitializeComponent();
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();
        }

        public PersonEventList(string ev_name, int id)
        {
            InitializeComponent();
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();

            _evName = ev_name;
            _idEv = id;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ev = await _eventsDataBase.GetEvByID(_idEv);
            buttonAddPerson.IsVisible = ev.IsNoEnded;
            closeEvent.IsVisible = ev.IsNoEnded;
            emptyFrame.IsVisible = ev.IsNoEnded;

            eventName.Text = _evName;
            string query = $"select * from ParticipantModel where id_event={_idEv}";
            var list = new List<ParticipantModel>(await _eventsDataBase.GetPartPerEvent(query));
            _parts = new ObservableCollection<ParticipantModel>(list);
            eventsPersonsList.ItemsSource = _parts;
        }

        private void eventsPersonsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            ((CollectionView)sender).SelectedItem = null;
        }

        private async void buttonAddPerson_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void closeEvent_Clicked(object sender, EventArgs e)
        {
            await _eventsDataBase.CloseAEvent(_idEv);
            await Navigation.PopAsync();
        }

        private async void buttonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

        }
    }
}