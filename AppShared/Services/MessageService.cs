using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppShared.Services
{
    public class MessageService
    {
        public async void Alert(string message)
        {
            await Application.Current.MainPage.DisplayAlert("提示", message, "OK");
        }

        public async Task<bool> DisplayConfirm(string message)
        {
            return await Application.Current.MainPage.DisplayAlert("提示", message, "是", "否");
        }
    }
}