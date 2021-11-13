using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using AppShared.Validators;
using AppShared.Validators.Rules;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.ViewModels;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeacherEnd.ViewModels
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : AccountViewModel
    {
        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            InitializeProperties();
            AddValidationRules();
            LoginCommand = new Command(LoginClicked);
            SignUpCommand = new Command(SignUpClicked);
        }


        public new ValidatablePair<string> Password { get; set; }


        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            Password = new ValidatablePair<string>();
        }


        /// <summary>
        /// this method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            Password.Item1.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "Password Required"});
            Password.Item2.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "Re-enter Password"});
        }

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoginClicked(object obj)
        {
            Application.Current.MainPage = new LoginPage();
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            UserName.Validate();
            Password.Validate();
            Phone.Validate();
            Email.Validate();

            if (UserName.IsValid && Password.IsValid && Phone.IsValid && Email.IsValid)
            {
                var response = await HttpClient.PostAsJsonAsync("/teacher/register", new 
                {
                    Id = UserName.Value,
                    Password = Password.Item1.Value,
                    Name = Name.Value,
                    Email = Email.Value,
                    PhoneNum = Phone.Value
                });
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content is not "")
                    {
                        MessageService.Alert(content);
                        return;
                    }

                    MessageService.Alert("Registered");
                }
            }
        }
    }
}