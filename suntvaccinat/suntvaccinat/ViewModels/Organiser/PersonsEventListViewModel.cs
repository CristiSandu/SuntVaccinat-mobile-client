using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Organiser
{
    public class PersonsEventListViewModel : BaseViewModel
    {
        public ObservableCollection<ParticipantModel> ParticipantsList { get; set; } = new ObservableCollection<ParticipantModel>();    

        public string EventName { get; set; }
        
        public int EventId { get; set; }
        
        public EventModel Event { get; set; }
        
        Services.IEventsDataBase _eventsDataBase;
        public ICommand CloseEvent { get; set; }


        public PersonsEventListViewModel(string eventName, int eventId)
        {
            EventName = eventName;
            EventId = eventId;
            _eventsDataBase = DependencyService.Get<Services.IEventsDataBase>();
            CloseEvent = new Command(async model =>
            {
                await _eventsDataBase.CloseAEvent(EventId);
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        public async void GetPersons()
        {
            Event = await _eventsDataBase.GetEvByID(EventId);
            OnPropertyChanged(nameof(Event));
            var users =  await _eventsDataBase.GetPartPerEvent(Helpers.DataBaseQuerys.GetParticipantsQuery(EventId));
            ParticipantsList = new ObservableCollection<ParticipantModel>(users);
            OnPropertyChanged(nameof(ParticipantsList));

        }
    }
}
