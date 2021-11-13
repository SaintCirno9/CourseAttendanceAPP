using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace TeacherEnd.ViewModels
{
    public class ExcusesListPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<Excuse> ExcusesList { get; set; } = new();
        public string SearchText { get; set; }

        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }

        public ExcusesListPageViewModel()
        {
            ItemTappedCommand = new Command<ItemTappedEventArgs>(NavigateToExcuseDetailPage);
        }

        public async Task RefreshList()
        {
            var excuses = await HttpClient.GetFromJsonAsync<List<Excuse>>("teacher/excuse/GetExcuses");
            if (excuses is not null)
            {
                ExcusesList.ReplaceRange(excuses);
            }
        }

        private void NavigateToExcuseDetailPage(ItemTappedEventArgs eventArgs)
        {
            ExcuseDetailPageViewModel.Excuse = eventArgs.ItemData as Excuse;
            Shell.Current.GoToAsync("ExcuseDetailPage");
        }
    }
}