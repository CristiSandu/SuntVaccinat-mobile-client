using suntvaccinat.Services;
using suntvaccinat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client.INSP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetINSPCertificatePage : ContentPage
    {
        
        IValidationServiceAPI _validationServiceApi;
        IDevice _getDeviceInfo;


        public GetINSPCertificatePage()
        {
            InitializeComponent();
            _validationServiceApi = DependencyService.Get<IValidationServiceAPI>();
            _getDeviceInfo = DependencyService.Get<IDevice>();
        }

        private static byte[] GetImageBytes(Stream stream)
        {
            byte[] ImageBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
        }

        private static Stream ConvertToStream(string fileUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                MemoryStream mem = new MemoryStream();
                Stream stream = response.GetResponseStream();

                stream.CopyTo(mem, 4096);


                return mem;
            }
            finally
            {
                response.Close();
            }
        }

        private async void SiteINSP_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains(".pdf") && !e.Url.Contains("instructiuni_covid_2021"))
            {
                Stream strim = ConvertToStream(e.Url);

                byte[] result = GetImageBytes(strim);

                string user = await SecureStorage.GetAsync(Helpers.Constants.User);
                string phoneNumber = await SecureStorage.GetAsync(Helpers.Constants.PhoneNumber);
                string phoneId = _getDeviceInfo.GetIdentifier();

                var request = new Services.RegisterCertificateRequest
                {
                    ValidationModel = new Services.INSPValidationModel
                    {
                        PdfBase64 = Convert.ToBase64String(result, 0, result.Length),
                        Name = user,
                        PhoneId = phoneId,
                        PhoneNumber = phoneNumber
                    }
                };

                var respons = await _validationServiceApi.ApiINSPAsync(request);

                e.Cancel = true;

                if (!respons.Status)
                {
                    await Application.Current.MainPage.DisplayAlert(Helpers.Constants.ErrorMsg, respons.Certificate, "Ok");
                    await Navigation.PopAsync();
                    DisplayAlert("Atentie!", "Numele, Prenumele, sex-ul sau varsta nu sunt introduse corect!", "OK");
                    return;
                }

                //Cancel Webview Navigation to stop it downloading the PDF as well!

                await Navigation.PopAsync();
            }
        }
    }
}