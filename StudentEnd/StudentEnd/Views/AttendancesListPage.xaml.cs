using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AppShared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.ViewModels;
using Syncfusion.DataSource.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace StudentEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendancesListPage : ContentPage
    {
        public AttendancesListPageViewModel ViewModel { get; set; }

        public AttendancesListPage()
        {
            InitializeComponent();
            this.AddBusyDisplay();
            ViewModel = BindingContext as AttendancesListPageViewModel;
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