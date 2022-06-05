using suntvaccinat.ViewModels.Client.INSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suntvaccinat.Views.Client.INSP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterINSPPage : ContentPage
    {
        public EnterINSPPage()
        {
            InitializeComponent();
            BindingContext = new EnterINSPPageViewModel();
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void DownloadINSPBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}