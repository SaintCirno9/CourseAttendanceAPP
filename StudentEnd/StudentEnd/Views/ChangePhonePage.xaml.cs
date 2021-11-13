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
    public partial class ChangePhonePage : ContentPage
    {
        public ChangePhonePageViewModel ViewModel { get; set; }

        public ChangePhonePage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ChangePhonePageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}