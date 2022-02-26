using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        public ObservableCollection<Models.EventModel> MyEvents { get; set; } = new ObservableCollection<Models.EventModel>();
        public Command<Models.EventModel> DeleteEvent { get; private set; }
        Services.IEventsDataBase _eventsDataBase;

        public EventsViewModel()
        {
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();

            DeleteEvent = new Command<Models.EventModel>(async model =>
            {
                MyEvents.Remove(model);
                await _eventsDataBase.RemoveEvent(model.id_event);
            });
        }
    }
}
