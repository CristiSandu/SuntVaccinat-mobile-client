using Microcharts;
using suntvaccinat.Models;
using suntvaccinat.Services.Interfaces;
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
        public bool IsChartVisible { get; set; }
        public int HeightFooter { get; set; }
        public EventModel Event { get; set; }
        
        IEventsDataBase _eventsDataBase;
        IStatsService _statService;
        public ICommand CloseEvent { get; set; }
        public Chart Chart { get; set; }

        public PersonsEventListViewModel(string eventName, int eventId)
        {
            EventName = eventName;
            EventId = eventId;
            _eventsDataBase = DependencyService.Get<IEventsDataBase>();
            _statService= DependencyService.Get<IStatsService>();

            CloseEvent = new Command(async model =>
            {
                await _eventsDataBase.CloseAEvent(EventId);
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        public async void GetPersons()
        {
            Event = await _eventsDataBase.GetEvByID(EventId);
            IsChartVisible = !Event.IsNoEnded;

            if (IsChartVisible)
            {
                Chart = await _statService.GenerateStatsForEvent(EventId);
                HeightFooter = 290;
                OnPropertyChanged(nameof(Chart));
            }else
            {
                HeightFooter = 0;
            }

            OnPropertyChanged(nameof(HeightFooter));
            OnPropertyChanged(nameof(Event));
            OnPropertyChanged(nameof(IsChartVisible));

            var users =  await _eventsDataBase.GetPartPerEvent(Helpers.DataBaseQuerys.GetParticipantsQuery(EventId));
            ParticipantsList = new ObservableCollection<ParticipantModel>(users);
            OnPropertyChanged(nameof(ParticipantsList));
        }
    }
}
