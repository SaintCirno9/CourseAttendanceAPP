using System.Net.Http;
using System.Net.Http.Json;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;

namespace TeacherEnd.ViewModels
{
    public class AccountInfoPageViewModel : DetailPageViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public AccountInfoPageViewModel()
        {
            PageTitle = "个人信息";
            GetInfo();
        }

        private async void GetInfo()
        {
            var teacher = await HttpClient.GetFromJsonAsync<Teacher>("teacher/getInfo");
            if (teacher is null)
            {
                return;
            }

            Id = teacher.Id;
            Name = teacher.Name;
            Email = teacher.Email;
            Phone = teacher.PhoneNum;
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Phone));
        }
    }
}