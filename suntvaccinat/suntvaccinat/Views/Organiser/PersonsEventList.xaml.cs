using ImageFromXamarinUI;
using suntvaccinat.Models;
using suntvaccinat.ViewModels.Organiser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Organiser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonsEventList : ContentPage
    {
        int _idEvent = 0;

        public PersonsEventList()
        {
            InitializeComponent();
        }

        public PersonsEventList(string eventName, int eventId)
        {
            InitializeComponent();
            _idEvent= eventId;
            BindingContext = new PersonsEventListViewModel(eventName, eventId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as PersonsEventListViewModel).GetPersons();
        }

        private async void ButtonAddPerson_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanPage { BindingContext = new ScanOrganiserViewModel(_idEvent) });
        }

        private async void ButtonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

       

        private async void ButtonShareStats_Clicked(object sender, EventArgs e)
        {
            EventNameStat.IsVisible = true;

            var imageStream = await StatsView.CaptureImageAsync();

            var directory = Path.Combine(FileSystem.AppDataDirectory, "ImageEditorSavedImages");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileFullPath = Path.Combine(directory, "MySavedImage.png");

            SaveStreamToFile(fileFullPath, imageStream);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                File = new ShareFile(fileFullPath)
            });

            EventNameStat.IsVisible = false;
        }

        public void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}