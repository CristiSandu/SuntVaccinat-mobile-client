using suntvaccinat.Models;
using suntvaccinat.ViewModels;
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
    public partial class EventsListPage : ContentPage
    {
        Services.IEventsDataBase _eventsDataBase;
        public EventsListPage()
        {
            InitializeComponent();
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();
            BindingContext = new EventsViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var query = $"select * from EventModel order by Date";
            var list = new List<EventModel>(await _eventsDataBase.GetEvents(query));
            (BindingContext as EventsViewModel).MyEvents = new ObservableCollection<Models.EventModel>(list.Reverse<EventModel>());
            eventsList.ItemsSource = (BindingContext as EventsViewModel).MyEvents;
        }

        private async void eventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            
            EventModel eventt = e.CurrentSelection.FirstOrDefault() as EventModel;
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new PersonEventList(eventt.Name, eventt.id_event));
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        private async void buttonEvent_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Add Event", "Name of Event: ");
            if (result != null)
                await _eventsDataBase.AddEvent(result, DateTime.Now);
            OnAppearing();
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SwipeItem_Clicked(object sender, EventArgs e)
        {
            EventModel eventModel = (EventModel)sender;
            await _eventsDataBase.RemoveEvent(eventModel.id_event);
        }
    }
}