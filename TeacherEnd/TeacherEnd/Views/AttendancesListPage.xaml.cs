using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace TeacherEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendancesListPage : ContentPage
    {
        public AttendancesListPageViewModel ViewModel { get; set; }

        public AttendancesListPage()
        {
            InitializeComponent();
            PullToRefreshContainer.Refreshing += PullToRefresh_Refreshing;
            ViewModel = BindingContext as AttendancesListPageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.RefreshList();
        }

        private async void PullToRefresh_Refreshing(object sender, EventArgs args)
        {
            PullToRefreshContainer.IsRefreshing = true;
            await ViewModel.RefreshList();
            PullToRefreshContainer.IsRefreshing = false;
        }
    }
}