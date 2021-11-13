using StudentEnd.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StudentEnd.Views
{

    [Preserve(AllMembers = true)]
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