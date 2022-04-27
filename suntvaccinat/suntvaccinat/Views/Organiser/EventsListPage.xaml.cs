﻿using suntvaccinat.Models;
using suntvaccinat.ViewModels;
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
    public partial class EventsListPage : ContentPage
    {
        public EventsListPage()
        {
            InitializeComponent();
            BindingContext = new EventsListPageViewModel();
        }

        private async void eventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            
            EventModel ev = e.CurrentSelection.FirstOrDefault() as EventModel;
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new PersonEventList(ev.Name, ev.id_event));
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}