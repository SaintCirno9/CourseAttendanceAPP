using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppShared.Extensions;
using StudentEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendanceRecordsListPage : ContentPage
    {
        public AttendanceRecordsListPageViewModel ViewModel { get; set; }

        public AttendanceRecordsListPage()
        {
            InitializeComponent();
            this.AddBusyDisplay();
            ViewModel = BindingContext as AttendanceRecordsListPageViewModel;
            PullToRefreshContainer.Refreshing += PullToRefresh_Refreshing;
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