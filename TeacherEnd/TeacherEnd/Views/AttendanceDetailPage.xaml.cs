using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TeacherEnd.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendanceDetailPage : ContentPage
    {
        public AttendanceDetailPageViewModel ViewModel { get; set; }

        public AttendanceDetailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as AttendanceDetailPageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.Init();
        }
    }
}