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
    public partial class CourseStudentInfoPage : ContentPage
    {
        public CourseStudentInfoPageViewModel ViewModel { get; set; }
        
        public CourseStudentInfoPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as CourseStudentInfoPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}