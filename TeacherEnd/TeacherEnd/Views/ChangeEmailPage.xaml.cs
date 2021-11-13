﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeEmailPage : ContentPage
    {
        public ChangeEmailPageViewModel ViewModel { get; set; }

        public ChangeEmailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ChangeEmailPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}