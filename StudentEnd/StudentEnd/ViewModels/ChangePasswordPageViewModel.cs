using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using AppShared.Validators;
using AppShared.Validators.Rules;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.Views;
using Xamarin.Forms;

namespace StudentEnd.ViewModels
{
    public class ChangePasswordPageViewModel : DetailPageViewModel
    {
        public ValidatablePair<string> Password { get; set; } = new();
        public ValidatableObject<string> VerifyCode { get; set; } = new();

        public string SendCodeButtonText { get; set; } = "发送";
        public bool SendCodeCommandCanExecute { get; set; } = true;
        public Command SendCodeCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command ResetCommand { get; set; }

        public ChangePasswordPageViewModel()
        {
            PageTitle = "修改密码";
            SendCodeCommand = new Command(SendCodeClicked, () => SendCodeCommandCanExecute);
            SubmitCommand = new Command(SubmitClicked);
            ResetCommand = new Command(ResetClicked);
        }

        public void Init()
        {
            Password = new ValidatablePair<string>
            {
                Item1 = new ValidatableObject<string>(),
                Item2 = new ValidatableObject<string>()
            };
            VerifyCode = new ValidatableObject<string>();
            Password.Item1.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "密码不能为空"});
            Password.Item2.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "新密码不能为空"});
            Password.ValidationRules.Add(new IsNotSameRule<ValidatablePair<string>>() {ValidationMessage = "新旧密码不能相同"});
            VerifyCode.ValidationRules.Add(new IsNotNullOrEmptyRule<string>() {ValidationMessage = "验证码不能为空"});

            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(VerifyCode));
        }

        public Timer Timer { get; set; }
        public int Count { get; set; } = 60;

        private async void SendCodeClicked()
        {
            var response = await HttpClient.GetAsync("mail/sendCode");
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
                MessageService.Alert("验证码发送失败");
            }
        }

        private async void SubmitClicked()
        {
            Password.Validate();
            VerifyCode.Validate();
            if (Password.IsValid && VerifyCode.IsValid)
            {
                var response = await HttpClient.PostAsJsonAsync("teacher/ChangePassword", new
                {
                    newPassword = Password.Item2.Value,
                    code = VerifyCode.Value
                });
                var message = await response.Content.ReadAsStringAsync();
                if (message != "")
                {
                    MessageService.Alert(message);
                }
                else
                {
                    MessageService.Alert("修改成功");
                    await LoginDatabaseService.DeleteAsync();
                    Application.Current.MainPage = new LoginPage();
                }
            }
        }

        private void ResetClicked()
        {
            Init();
        }
    }
}