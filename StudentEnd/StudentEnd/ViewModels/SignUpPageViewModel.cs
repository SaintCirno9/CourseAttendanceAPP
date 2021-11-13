using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using AppShared.Validators;
using AppShared.Validators.Rules;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.ViewModels;
using StudentEnd.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StudentEnd.ViewModels
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : AccountViewModel
    {
        public new ValidatablePair<string> Password { get; set; } = new()
        {
            Item1 = new ValidatableObject<string>()
            {
                ValidationRules = { new IsNotNullOrEmptyRule<string>
                    {ValidationMessage = "Password Required"}}
            },
            Item2 = new ValidatableObject<string>()
            {
                ValidationRules = { new IsNotNullOrEmptyRule<string>
                    {ValidationMessage = "Re-enter Password"}}
            }
        };
        public Command LoginCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public SignUpPageViewModel()
        {
            LoginCommand = new Command(LoginClicked);
            SignUpCommand = new Command(SignUpClicked);
        }
        
        private void LoginClicked(object obj)
        {
            Application.Current.MainPage = new LoginPage();
        }

        private async void SignUpClicked(object obj)
        {
            UserName.Validate();
            Password.Validate();
            Phone.Validate();
            Email.Validate();

            if (!UserName.IsValid || !Password.IsValid || !Phone.IsValid || !Email.IsValid)
            {
                return;
            }
            var imageResult = await MediaPicker.CapturePhotoAsync();
            if (imageResult is null)
            {
                return;
            }

            var faceImageStream = await imageResult.OpenReadAsync();

            var faceImageData = new byte[faceImageStream.Length];
            await faceImageStream.ReadAsync(faceImageData, 0, faceImageData.Length);
            faceImageStream.Close();
            var faceData = Convert.ToBase64String(faceImageData);
            
            var response = await HttpClient.PostAsJsonAsync("/student/register", new
            {
                Id = UserName.Value,
                Password = Password.Item1.Value,
                Name = Name.Value,
                Email = Email.Value,
                PhoneNum = Phone.Value,
                FaceData = faceData
            });
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("注册成功");
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content is not "")
                {
                    MessageService.Alert(content);
                }
            }
        }
    }
}