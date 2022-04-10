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
        public GetINSPCertificatePage()
        {
            InitializeComponent();
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
            if (e.Url.Contains(".pdf"))
            {
                Stream strim = ConvertToStream(e.Url);

                byte[] result = GetImageBytes(strim);
                string user = await SecureStorage.GetAsync("User");

                Models.ResultModel outpdf = await Services.ValidationCertificate.GetDataToCompute(strim);

                //Cancel Webview Navigation to stop it downloading the PDF as well!
                e.Cancel = true;
                if (!outpdf.Find)
                {
                    await Navigation.PopAsync();
                    DisplayAlert("Atentie!", "Numele, Prenumele, sex-ul sau varsta nu sunt introduse corect!", "OK");
                    return;
                }


                await Navigation.PopAsync();
            }
        }
    }
}