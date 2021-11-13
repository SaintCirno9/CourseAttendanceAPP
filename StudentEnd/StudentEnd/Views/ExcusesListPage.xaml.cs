﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace StudentEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcusesListPage : ContentPage
    {
        
        public ExcusesListPageViewModel ViewModel { get; set; }
        public ExcusesListPage()
        {
            
            InitializeComponent();
            PullToRefreshContainer.Refreshing += PullToRefresh_Refreshing;
            ViewModel = BindingContext as ExcusesListPageViewModel;
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