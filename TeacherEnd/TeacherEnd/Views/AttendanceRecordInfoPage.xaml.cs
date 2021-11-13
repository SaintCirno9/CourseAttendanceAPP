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
    public partial class AttendanceRecordInfoPage : ContentPage
    {
        public AttendanceRecordInfoPageViewModel ViewModel { get; set; }
        public AttendanceRecordInfoPage()
        {
            InitializeComponent();
            ViewModel= BindingContext as AttendanceRecordInfoPageViewModel;;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}