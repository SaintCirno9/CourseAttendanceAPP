using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

using StudentEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StudentEnd.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
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
                response = await HttpClient.PostAsJsonAsync("student/login", new
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
                    var responseDict = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    if (responseDict is null)
                    {
                        MessageService.Alert("登录失败");
                        return;
                    }

                    var token = responseDict["token"];
                    HttpClient.DefaultRequestHeaders.Authorization =
                        AuthenticationHeaderValue.Parse("Bearer " + token);

                    await LoginDatabaseService.SaveAsync(id, password);
                    App.StudentId = id;
                    App.StudentFace = responseDict.ContainsKey("face") ? responseDict["face"] : "";
                    Application.Current.MainPage = new AppShell();
                    return;
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