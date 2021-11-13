using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using CommonShared.DataModels;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcuseDetailPage : ContentPage
    {
        public ExcuseDetailPageViewModel ViewModel { get; set; }

        public ExcuseDetailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ExcuseDetailPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ExcuseDetailPageViewModel.Excuse is {ExcuseState: ExcuseState.Unreviewed})
            {
                AcceptButton.IsVisible = true;
                RejectButton.IsVisible = true;
            }
            ViewModel.Init();
        }
    }
}