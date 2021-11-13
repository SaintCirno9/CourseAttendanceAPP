using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfoPage : ContentPage
    {
        public AccountInfoPageViewModel ViewModel { get; set; }

        public AccountInfoPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as AccountInfoPageViewModel;
        }
    }
}