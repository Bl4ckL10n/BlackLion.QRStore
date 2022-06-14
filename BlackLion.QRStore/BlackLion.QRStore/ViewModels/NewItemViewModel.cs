using BlackLion.QRStore.Helpers;
using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using System;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    public class NewItemViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly IMessageService _messageService;
        private bool isValidURL;
        private string name;
        private string url;

        public bool IsValidURL
        {
            get => isValidURL;
            set => SetProperty(ref isValidURL, value);
        }
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
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            Title = "New Item";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            if (url == null)
            {
                return false;
            }

            IsValidURL = URLValidatorHelper.IsValidURL(URL);

            return !String.IsNullOrWhiteSpace(url) &&
                !String.IsNullOrWhiteSpace(name) &&
                isValidURL;
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
            var duplicatedItem = (await _dataStore.GetItemsAsync()).Find(item => item.URL == url);

            if (duplicatedItem != null)
            {
                await _messageService.ShowAsync(
                    "Duplicated entry",
                    "There is an entry for that URL already.",
                    "Ok"
                );
            }
            else
            {
                Item newItem = new Item()
                {
                    Name = name,
                    URL = url
                };

                await _dataStore.AddItemAsync(newItem);
                await Shell.Current.GoToAsync("//ItemsPage");
            }
        }
    }
}
