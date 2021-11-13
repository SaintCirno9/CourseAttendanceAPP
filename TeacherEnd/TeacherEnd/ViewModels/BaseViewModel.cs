using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using AppShared.Services;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public readonly HttpClient HttpClient = App.ServiceProvider.GetRequiredService<HttpClient>();
        public readonly MessageService MessageService = App.ServiceProvider.GetRequiredService<MessageService>();

        public readonly LoginDatabaseService LoginDatabaseService =
            App.ServiceProvider.GetRequiredService<LoginDatabaseService>();

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}