using BlackLion.QRStore.Models;
using System;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    public class NewItemViewModel : BaseViewModel, IQueryAttributable
    {
        private string name;
        private string url;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string URL
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        public NewItemViewModel()
        {
            Title = "New Item";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(url);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            URL = HttpUtility.UrlDecode(query["url"]);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("//ItemsPage");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Name = Name,
                URL = URL
            };

            await App.Database.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("//ItemsPage");
        }
    }
}
