using System.Net;
using System.Net.Http;
using CommonShared.DataModels;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.ViewModels
{
    public class ExcuseDetailPageViewModel : DetailPageViewModel
    {
        public static Excuse Excuse { get; set; }
        public string ExcuseReason { get; set; }
        public string ExcusePeriod { get; set; }
        public string ExcuseCourseName { get; set; }


        public Command RejectCommand { get; set; }
        public Command AcceptCommand { get; set; }

        public ExcuseDetailPageViewModel()
        {
            PageTitle = "请假信息";
            AcceptCommand = new Command(AcceptClicked);
            RejectCommand = new Command(RejectClicked);
        }

        public void Init()
        {
            if (Excuse is not null)
            {
                ExcusePeriod = Excuse.Period;
                ExcuseReason = Excuse.Reason;
                ExcuseCourseName = Excuse.Course.Name;
                OnPropertyChanged(nameof(ExcusePeriod));
                OnPropertyChanged(nameof(ExcuseReason));
                OnPropertyChanged(nameof(ExcuseCourseName));
            }
        }

        private void AcceptClicked()
        {
            var response = HttpClient
                .GetAsync($"teacher/excuse/ReviewExcuse/{Excuse.Id}/{ExcuseState.Passed}").Result;


            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("操作成功");
                BackClicked();
                return;
            }

            MessageService.Alert("操作失败");
        }

        private void RejectClicked()
        {
            var response = HttpClient
                .GetAsync($"teacher/excuse/ReviewExcuse/{Excuse.Id}/{ExcuseState.Rejected}")
                .Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("操作成功");
                BackClicked();
                return;
            }

            MessageService.Alert("操作失败");
        }
    }
}