using Newtonsoft.Json;
using suntvaccinat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace suntvaccinat.ViewModels.Client.INSP
{
    public class EnterINSPPageViewModel : BaseViewModel
    {
        private string _certificate = "Select from files";

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

        public ICommand SaveCertificateCommand { get; set; }
        public IDevice _getDeviceInfo;
        public IEventsDataBase _database;
        public Services.IValidationServiceAPI _validationServiceApi;

        private bool _used = false;
        public EnterINSPPageViewModel()
        {
            _validationServiceApi = DependencyService.Get<Services.IValidationServiceAPI>();
            _getDeviceInfo = DependencyService.Get<IDevice>();


            SaveCertificateCommand = new Command(async () =>
           {
               var customFileType =
                    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                         { DevicePlatform.iOS, new[] { "com.adobe.pdf" } }, // or general UTType values
                         { DevicePlatform.Android, new[] { "application/pdf" } },
                         { DevicePlatform.UWP, new[] { ".pdf" } },
                         { DevicePlatform.Tizen, new[] { "*/*" } },
                         { DevicePlatform.macOS, new[] { "pdf"} }, // or general UTType values
                    });

               var pickResult = await FilePicker.PickAsync(new PickOptions
               {
                   FileTypes = customFileType,
                   PickerTitle = "Pick an PDF"
               });

               if (pickResult != null)
               {
                   var stream = await pickResult.OpenReadAsync();

                   MemoryStream ms = new MemoryStream();
                   stream.CopyTo(ms);
                   byte[] value = ms.ToArray();
                   var result = Convert.ToBase64String(value, 0, value.Length);


                   string user = await SecureStorage.GetAsync(Helpers.Constants.User);
                   string phoneNumber = await SecureStorage.GetAsync(Helpers.Constants.PhoneNumber);

                   string phoneId = _getDeviceInfo.GetIdentifier();


                   var request = new Services.RegisterCertificateRequest
                   {
                       ValidationModel = new Services.INSPValidationModel
                       {
                           PdfBase64 = Convert.ToBase64String(value, 0, value.Length),
                           Name = user,
                           PhoneId = phoneId,
                           PhoneNumber = phoneNumber
                       }
                   };

                   var text = JsonConvert.SerializeObject(request);
                   var respons = await _validationServiceApi.ApiINSPAsync(request);

                   if (!respons.Status)
                   {
                       await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, respons.Certificate, "Ok");
                       return;
                   }

                   await SecureStorage.SetAsync(Helpers.Constants.INSPPref, $"{respons.Certificate}////{phoneId}");
                   Preferences.Set(Helpers.Constants.INSPPref, true);

                   await Application.Current.MainPage.DisplayAlert(Helpers.Constants.SuccessMsg, "INSP Certificate saved", "Ok");
                   await Application.Current.MainPage.Navigation.PopToRootAsync();
               }
           });
        }
    }
}
