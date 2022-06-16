using BlackLion.QRStore.Helpers;
using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using System;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    [QueryProperty(nameof(ItemId), "id")]
    public class EditItemViewModel : BaseViewModel
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly IMessageService _messageService;
        private bool isValidURL;
        private int itemId;
        private Item item;
        private string name;
        private string url;

        public Command CancelCommand { get; }

        public Command SaveCommand { get; }

        public bool IsValidURL
        {
            get => isValidURL;
            set => SetProperty(ref isValidURL, value);
        }

        public int ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(value);
            }
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

        public EditItemViewModel()
        {
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            Title = "Edit";
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave, ValidateSave);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async void OnSave(object obj)
        {
            item.Name = name;
            item.URL = url;
            var duplicatedItem = (await _dataStore.GetItemsAsync()).Find(item => item.URL == url && item.Id != itemId);

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
                var isUpdated = await _dataStore.UpdateItemAsync(item);

                if (!isUpdated)
                {
                    return;
                }

                await Shell.Current.GoToAsync("..");
            }
        }

        private bool ValidateSave(object arg)
        {
            if (item == null || url == null)
            {
                return false;
            }

            IsValidURL = URLValidatorHelper.IsValidURL(url);

            return !String.IsNullOrWhiteSpace(url) &&
                !String.IsNullOrWhiteSpace(name) &&
                (item.Name != name ||
                item.URL != url) &&
                isValidURL;
        }

        private async void OnCancel(object obj)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void LoadItemId(int itemId)
        {
            try
            {
                item = await _dataStore.GetItemAsync(itemId);
                Name = item.Name;
                URL = item.URL;
            }
            catch (Exception)
            {
            }
        }
    } 
}
