using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using AppShared.Extensions;
using StudentEnd.ViewModels;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StudentEnd.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesListPage
    {
        public CoursesListPageViewModel ViewModel { get; set; }

        public CoursesListPage()
        {
            InitializeComponent();
            this.AddBusyDisplay();
            ViewModel = BindingContext as CoursesListPageViewModel;
            PullToRefreshContainer.Refreshing += PullToRefresh_Refreshing;
        }

        private async void PullToRefresh_Refreshing(object sender, EventArgs args)
        {
            PullToRefreshContainer.IsRefreshing = true;
            await ViewModel.RefreshList();
            PullToRefreshContainer.IsRefreshing = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.RefreshList();
        }
    }
}