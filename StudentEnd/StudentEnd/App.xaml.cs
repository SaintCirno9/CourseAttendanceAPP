using System;
using StudentEnd.Views;
using Syncfusion.Licensing;
using Xamarin.Forms;

namespace StudentEnd
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static string StudentId { get; set; }
        public static string StudentFace { get; set; }

        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense(
                "NDMyMTY4QDMxMzkyZTMxMmUzMG80cjFwUUtHaEpNbXJ6QmE4S1lxNmpEdzZ3NEtSbktDZDRvZUQzdzZwcFk9");
            InitializeComponent();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}