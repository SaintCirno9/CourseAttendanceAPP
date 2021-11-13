using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPageViewModel ViewModel { get; set; }

        public ChangePasswordPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ChangePasswordPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}