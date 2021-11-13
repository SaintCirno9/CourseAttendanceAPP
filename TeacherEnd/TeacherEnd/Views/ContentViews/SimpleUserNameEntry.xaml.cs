using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppShared.Validators;
using AppShared.Validators.Rules;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleUserNameEntry : ContentView
    {
        public ValidatableObject<string> UserName { get; set; } = new();

        public SimpleUserNameEntry()
        {
            UserName.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "UserName Required"});
            InitializeComponent();
        }
    }
}