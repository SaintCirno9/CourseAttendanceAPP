using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        public LoginPageViewModel ViewModel { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as LoginPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.UseDatabaseToLogin();
        }
    }
}