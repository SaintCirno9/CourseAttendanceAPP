using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StudentEnd.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ProfilePageViewModel : BaseViewModel
    {
        public Command ViewProfileCommand { get; set; }
        public Command ChangePasswordCommand { get; set; }
        public Command ChangeEmailCommand { get; set; }
        public Command ChangePhoneCommand { get; set; }
        public Command LogoutCommand { get; set; }

        public ProfilePageViewModel()
        {
            ViewProfileCommand = new Command(ViewProfileClicked);
            ChangePasswordCommand = new Command(ChangePasswordClicked);
            ChangeEmailCommand = new Command(ChangeEmailClicked);
            ChangePhoneCommand = new Command(ChangePhoneClicked);
            LogoutCommand = new Command(LogoutClicked);
        }


        private void ViewProfileClicked(object obj)
        {
            Shell.Current.GoToAsync(nameof(AccountInfoPage));
        }

        private void ChangePasswordClicked(object obj)
        {
            Shell.Current.GoToAsync(nameof(ChangePasswordPage));
        }

        private void ChangeEmailClicked(object obj)
        {
            Shell.Current.GoToAsync(nameof(ChangeEmailPage));
        }

        private void ChangePhoneClicked(object obj)
        {
            Shell.Current.GoToAsync(nameof(ChangePhonePage));
        }

        private void LogoutClicked(object obj)
        {
            LoginDatabaseService.DeleteAsync();
            Application.Current.MainPage = new LoginPage();
        }
    }
}