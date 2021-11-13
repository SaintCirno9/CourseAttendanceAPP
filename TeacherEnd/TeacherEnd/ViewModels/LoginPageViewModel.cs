using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.Views;
using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class LoginPageViewModel : AccountViewModel
    {
        public bool ShowLoginPage { get; set; }

        public Command LoginCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public Command ForgotPasswordCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(LoginClicked);
            SignUpCommand = new Command(SignUpClicked);
            ForgotPasswordCommand = new Command(ForgotPasswordClicked);
        }

        public async void UseDatabaseToLogin()
        {
            if (await LoginDatabaseService.GetAsync() is { } loginData)
            {
                TryLogin(loginData.Id, loginData.Password);
            }
            else
            {
                ShowLoginPage = true;
                OnPropertyChanged(nameof(ShowLoginPage));
            }
        }

        private void LoginClicked(object obj)
        {
            UserName.Validate();
            Password.Validate();
            if (UserName.IsValid && Password.IsValid)
            {
                TryLogin(UserName.Value, Password.Value);
            }
        }

        private async void TryLogin(string id, string password)
        {
            HttpResponseMessage response;
            try
            {
                response = await HttpClient.PostAsJsonAsync("teacher/login", new
                {
                    id,
                    password
                });
            }
            catch (Exception e)
            {
                MessageService.Alert("登陆失败，网络连接错误");
                return;
            }
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var token = await response.Content.ReadAsStringAsync();
                    HttpClient.DefaultRequestHeaders.Authorization =
                        AuthenticationHeaderValue.Parse("Bearer " + token);

                    await LoginDatabaseService.SaveAsync(id, password);
                    Application.Current.MainPage = new AppShell();
                    break;
                }
                case HttpStatusCode.Unauthorized:
                    if (ShowLoginPage)
                    {
                        MessageService.Alert(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        await LoginDatabaseService.DeleteAsync();
                        ShowLoginPage = true;
                        OnPropertyChanged(nameof(ShowLoginPage));
                    }

                    break;
            }
        }

        private void SignUpClicked(object obj)
        {
            Application.Current.MainPage = new SignUpPage();
        }

        private void ForgotPasswordClicked(object obj)
        {
            Application.Current.MainPage = new ForgotPasswordPage();
        }
    }
}