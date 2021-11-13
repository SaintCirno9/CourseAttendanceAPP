using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using StudentEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

namespace StudentEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcuseDetailPage : ContentPage
    {
        public ExcuseDetailPageViewModel ViewModel { get; set; }

        public ExcuseDetailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ExcuseDetailPageViewModel;
        }

        protected override async void OnAppearing()
        {
            await ViewModel.Init();
            base.OnAppearing();
        }
    }
}