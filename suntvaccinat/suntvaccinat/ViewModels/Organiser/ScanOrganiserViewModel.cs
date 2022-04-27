﻿using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace suntvaccinat.ViewModels.Organiser
{
    public class ScanOrganiserViewModel : BaseViewModel
    {
        private string _certificate = string.Empty;

        public string Certificate
        {
            get
            {
                return _certificate;
            }
            set
            {
                _certificate = value;
                OnPropertyChanged(nameof(Certificate));
            }
        }
        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set
            {
                if (!Equals(_isAnalyzing, value))
                {
                    _isAnalyzing = value;
                    OnPropertyChanged(nameof(IsAnalyzing));

                }
            }
        }

        private bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (!Equals(_isScanning, value))
                {
                    _isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                }
            }
        }
        public Result Result { get; set; }

        Services.IDevice _getDeviceInfo;
        Services.IEventsDataBase _database;
        Services.IValidationServiceAPI _validationServiceApi;

        private bool _used = false;
        public ICommand ScanCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Certificate = Result.Text;
                        string[] elements = Certificate.Split(new string[] { "////" }, StringSplitOptions.None);

                        string phoneId = elements.Length == 2 ? elements[1] : null;
                        string certificate = elements.Length == 2 ? elements[0] : null;

                        if (_used)
                            return;

                        if (elements.Length == 0)
                        {
                            await App.Current.MainPage.DisplayAlert("Error", "Invalid Certificate format", "OK");
                            return;
                        }

                        if (!string.IsNullOrEmpty(Certificate) && Certificate.StartsWith("HC1:"))
                        {
                            _used = true;
                            var decodedValue = await Services.ValidationCertificate.DecodeGreenPass(certificate);
                            var valModelRespons = await Services.ValidationCertificate.GetValueToCheckWithServer(decodedValue, phoneId);
                            var checkCertificate = await _validationServiceApi.ApiValidationCheckIfExistDocumentsAsync(valModelRespons.DocumentId);
                            if (!checkCertificate)
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "Invalid Certificate pair", "OK");
                                return;
                            }

                            await _database.AddUserToEvent(new ParticipantModel
                            {
                                Name = $"{ decodedValue.Dgc.Name.FamilyName}-{decodedValue.Dgc.Name.GivenName}:{decodedValue.Dgc.DateOfBirth}",
                                id_event = EventId
                            });

                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                    });

                    IsAnalyzing = true;
                    IsScanning = true;
                });
            }
        }

        public int EventId { get; set; }
        public ScanOrganiserViewModel(int _idEv)
        {
            EventId = _idEv;
            _getDeviceInfo = DependencyService.Get<Services.IDevice>();
            _database = DependencyService.Get<Services.IEventsDataBase>();
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();
        }
    }
}