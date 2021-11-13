using System.Net;
using System.Net.Http.Json;
using System.Timers;
using AppShared.Validators;
using AppShared.Validators.Rules;
using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class ChangeEmailPageViewModel : DetailPageViewModel
    {
        public ValidatableObject<string> NewEmail { get; set; } = new();
        public ValidatableObject<string> VerifyCode { get; set; } = new();
        public string SendCodeButtonText { get; set; } = "发送";
        public bool SendCodeCommandCanExecute { get; set; } = true;
        public Command SendCodeCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command ResetCommand { get; set; }

        public ChangeEmailPageViewModel()
        {
            PageTitle = "修改邮箱";
            SendCodeCommand = new Command(SendCodeClicked, () => SendCodeCommandCanExecute);
            SubmitCommand = new Command(SubmitClicked);
            ResetCommand = new Command(ResetClicked);
        }

        public void Init()
        {
            NewEmail = new ValidatableObject<string>();
            VerifyCode = new ValidatableObject<string>();
            NewEmail.ValidationRules.Add(new IsNotNullOrEmptyRule<string>() {ValidationMessage = "邮箱不能为空"});
            NewEmail.ValidationRules.Add(new IsValidEmailRule<string>() {ValidationMessage = "邮箱格式错误"});
            VerifyCode.ValidationRules.Add(new IsNotNullOrEmptyRule<string>() {ValidationMessage = "验证码不能为空"});
            OnPropertyChanged(nameof(NewEmail));
            OnPropertyChanged(nameof(VerifyCode));
        }

        public Timer Timer { get; set; }
        public int Count { get; set; } = 60;

        private async void SendCodeClicked()
        {
            NewEmail.Validate();
            if (!NewEmail.IsValid)
            {
                return;
            }

            var response = await HttpClient.GetAsync($"mail/sendCode/{NewEmail.Value}");
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
            NewEmail.Validate();
            VerifyCode.Validate();
            if (NewEmail.IsValid && VerifyCode.IsValid)
            {
                var response = await HttpClient.PostAsJsonAsync("teacher/ChangeEmail", new
                {
                    newEmail = NewEmail.Value,
                    code = VerifyCode.Value
                });
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MessageService.Alert("修改成功");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    MessageService.Alert(message);
                }
            }
        }

        private void ResetClicked()
        {
            Init();
        }
    }
}