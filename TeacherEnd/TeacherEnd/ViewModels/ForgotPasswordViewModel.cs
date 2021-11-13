using System.Net;
using System.Net.Http.Json;
using System.Timers;
using AppShared.Validators;
using AppShared.Validators.Rules;
using TeacherEnd.ViewModels;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeacherEnd.ViewModels
{
    /// <summary>
    /// ViewModel for forgot password page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ForgotPasswordViewModel : AccountViewModel
    {
         public string SendCodeButtonText { get; set; } = "发送";
        public bool SendCodeCommandCanExecute { get; set; } = true;
        public ValidatableObject<string> VerifyCode { get; set; } = new()
        {
            ValidationRules =
            {
                new IsNotNullOrEmptyRule<string>
                    {ValidationMessage = "验证码不能为空"}
            }
        };
        public Command SendCodeCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            SignUpCommand = new Command(SignUpClicked);
            SendCodeCommand = new Command(SendCodeClicked, ()=>SendCodeCommandCanExecute);
            SubmitCommand = new Command(SubmitClicked);
        }

        public Timer Timer { get; set; }
        public int Count { get; set; } = 60;
        private async void SendCodeClicked()
        {
            Email.Validate();
            if (!Email.IsValid)
            {
                return;
            }
            var response = await HttpClient.GetAsync($"mail/ResetPasswordMail/TeacherEnd/{Email.Value}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                SendCodeCommandCanExecute = false;
                SendCodeCommand.ChangeCanExecute();
                OnPropertyChanged(nameof(SendCodeCommandCanExecute));
                Timer = new Timer(1000);
                Timer.Elapsed += (_, _) =>
                {
                    if (Count == 0)
                    {
                        Timer.Stop();
                        SendCodeButtonText = "发送";
                        SendCodeCommandCanExecute = true;
                        OnPropertyChanged(nameof(SendCodeCommandCanExecute));
                        Count = 60;
                        Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                            SendCodeCommand.ChangeCanExecute());
                    }
                    else
                    {
                        Count--;
                        SendCodeButtonText = $"{Count}s";
                    }

                    OnPropertyChanged(nameof(SendCodeButtonText));
                };
                Timer.Start();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                MessageService.Alert($"验证码发送失败:{content}");
            }
        }

        private async void SubmitClicked()
        {
            Email.Validate();
            Password.Validate();
            VerifyCode.Validate();
            if (!Email.IsValid || !Password.IsValid || !VerifyCode.IsValid)
            {
                return;
            }

            var response = await HttpClient.PostAsJsonAsync("teacher/resetPassword", new
            {
                code = VerifyCode.Value,
                email = Email.Value,
                password = Password.Value
            });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert($"重置密码成功");
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                MessageService.Alert($"重置密码失败:{content}");  
            }
        }
        
        private void SignUpClicked(object obj)
        {
            Application.Current.MainPage = new SignUpPage();
        }
    }
}