using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace StudentEnd.ViewModels
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
        }

        public async Task Init()
        {
            var student = await HttpClient.GetFromJsonAsync<Student>("student/getInfo");
            if (student is not null)
            {
                Id = student.Id;
                Name = student.Name;
                Email = student.Email;
                Phone = student.PhoneNum;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Phone));
            }
        }
    }
}