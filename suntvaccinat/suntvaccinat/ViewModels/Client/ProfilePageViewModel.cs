﻿using suntvaccinat.Models;
using suntvaccinat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Client
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public User User { get; set; }
        public string PhoneMode { get; set; }
        public string GPCertificate { get; set; } = "--";
        public string INSPCertificate { get; set; } = "--";
        public bool IsShown { get; set; } = false;

        public string SelectedCertificate { get; set; }

        IEventsDataBase _eventsDataBase;
        Services.IValidationServiceAPI _validationServiceApi;

        public ICommand ShowCertCommand { get; set; }

        public string SelectedCertLabel { get; set; }


        public ProfilePageViewModel()
        {
            _eventsDataBase = DependencyService.Get<IEventsDataBase>();

            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();

            GetCertificatesAsync();

            ShowCertCommand = new Command<string>((elem) =>
           {
               IsShown = !IsShown;
               if (elem != "-")
               {
                   if (elem == "GP")
                   {
                       SelectedCertificate = GPCertificate;
                       SelectedCertLabel = " Green Pass ";
                   }
                   else
                   {
                       SelectedCertificate = "--";
                       SelectedCertLabel = " INSP ";
                   }
               }

               OnPropertyChanged(nameof(SelectedCertLabel));
               OnPropertyChanged(nameof(SelectedCertificate));
               OnPropertyChanged(nameof(IsShown));
           });
        }

        public async void GetCertificatesAsync()
        {
            GPCertificate = await SecureStorage.GetAsync(Helpers.Constants.GreenPass);
            INSPCertificate = await SecureStorage.GetAsync(Helpers.Constants.INSPPref);

            if (INSPCertificate == null)
            {
                INSPCertificate = "--";
            }

            var device = DeviceInfo.Model;
            var manufacturer = DeviceInfo.Manufacturer;
            PhoneMode = $"{manufacturer} - {device}";

            User = await _eventsDataBase.GetUser();

            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(PhoneMode));
            OnPropertyChanged(nameof(GPCertificate));
            OnPropertyChanged(nameof(INSPCertificate));
        }
    }
}
