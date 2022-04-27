using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Organiser
{
    public class EventsListPageViewModel : BaseViewModel
    {
        public ObservableCollection<EventModel> MyEvents { get; set; } = new ObservableCollection<EventModel>();
        public Command<Models.EventModel> DeleteEvent { get; private set; }
        public ICommand CreateEvent { get; set; }

        Services.IEventsDataBase _eventsDataBase;

        public EventsListPageViewModel()
        {
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();
            PopulateEventsList();
            DeleteEvent = new Command<Models.EventModel>(async model =>
            {
                MyEvents.Remove(model);
                await _eventsDataBase.RemoveEvent(model.id_event);
            });

            CreateEvent = new Command(async model =>
            {
                string result = await App.Current.MainPage.DisplayPromptAsync("Add Event", "Name of Event: ");
                if (result != null)
                {
                    await _eventsDataBase.AddEvent(result, DateTime.Now);
                    PopulateEventsList();
                }
            });
        }

        public async void PopulateEventsList()
        {
            var eventsList = await _eventsDataBase.GetEvents(Helpers.DataBaseQuerys.GetEventsOrderByDate);
            var events = new List<EventModel>(eventsList);
            events.Reverse();
            MyEvents = new ObservableCollection<EventModel>(events);
            OnPropertyChanged(nameof(MyEvents));
        }
    }
}
