using System;
using System.Threading.Tasks;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesListPage
    {
        public CoursesListPageViewModel ViewModel { get; set; }

        public CoursesListPage()
        {
            Shell.SetNavBarIsVisible(this, false);
            InitializeComponent();
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