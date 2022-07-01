using Newtonsoft.Json;
using suntvaccinat.Models;
using suntvaccinat.Services;
using suntvaccinat.Services.Interfaces;
using System;
using System.Windows.Input;
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
                if (_isAnalyzing != value)
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
                if (_isScanning != value)
                {
                    _isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        public bool IsLoading { get => !_isScanning; }

        public Result Result { get; set; }

        IEventsDataBase _database;
        IValidationServiceAPI _validationServiceApi;
        IStatsService _statsService;

        private bool _used = false;
        public ICommand ScanCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Certificate = Result.Text;
                    string[] elements = Certificate.Split(new string[] { "////" }, StringSplitOptions.None);

                    string phoneId = elements.Length == 2 ? elements[1] : null;
                    string certificate = elements.Length == 2 ? elements[0] : null;

                    if (_used)
                        return;

                    if (elements.Length == 1)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.Current.MainPage.DisplayAlert("Error", "Invalid Certificate format", "OK");
                            await App.Current.MainPage.Navigation.PopAsync();
                            return;
                        });
                    }

                    if (!string.IsNullOrEmpty(certificate) && certificate.StartsWith("HC1:"))
                    {
                        _used = true;
                        var decodedValue = await ValidationCertificate.DecodeGreenPassPersonal(certificate);
                        var valModelRespons = ValidationCertificate.GetValueToCheckWithServer(decodedValue, phoneId);
                        var checkCertificate = await _validationServiceApi.ApiValidationCheckIfExistDocumentsAsync(valModelRespons.DocumentId, false);
                        if (!checkCertificate)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "Invalid Certificate pair", "OK");
                            });
                            return;
                        }

                        DateTime oDate = DateTime.Parse(decodedValue.Body.Content.DateOfBirth);
                        int age = DateTime.Now.Year - oDate.Year;

                        await _statsService.AddNewUserToStat(age, EventId);
                        await _database.AddUserToEvent(new ParticipantModel
                        {
                            Name = $"{decodedValue.Body.Content.Name.Surname}-{decodedValue.Body.Content.Name.Forename}:{decodedValue.Body.Content.DateOfBirth}",
                            id_event = EventId
                        });

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        });

                        return;
                    }

                    if (!string.IsNullOrEmpty(certificate))
                    {
                        var decodedCertificate = ValidationCertificate.DecodeINSP(certificate);
                        var userInfoINSP = JsonConvert.DeserializeObject<UserINSPModel>(decodedCertificate);
                        var checkCertificate = await _validationServiceApi.ApiValidationCheckIfExistDocumentsAsync($"{phoneId}-{userInfoINSP.ID}", true);

                        if (!checkCertificate)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Invalid Certificate pair", "OK");
                            });
                            return;
                        }

                        int age = Convert.ToInt32(userInfoINSP.Age);

                        await _statsService.AddNewUserToStat(age, EventId);
                        await _database.AddUserToEvent(new ParticipantModel
                        {
                            Name = $"{ userInfoINSP.Name}-{userInfoINSP.SecondName}:{userInfoINSP.Age}",
                            id_event = EventId
                        });

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        });
                    }

                    IsAnalyzing = true;
                    IsScanning = true;
                });
            }
        }

        public int EventId { get; set; }
        public ScanOrganiserViewModel(int _idEv)
        {
            EventId = _idEv;
            _statsService = DependencyService.Get<IStatsService>();
            _database = DependencyService.Get<IEventsDataBase>();
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();
        }
    }
}
