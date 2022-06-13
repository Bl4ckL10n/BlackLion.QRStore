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
        private int itemId;
        private Item item;
        private string name;
        private string url;

        public Command CancelCommand { get; }

        public Command SaveCommand { get; }
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
            Title = "Edit";
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave, ValidateSave);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async void OnSave(object obj)
        {
            item.Name = Name;
            item.URL = URL;
            var isUpdated = await _dataStore.UpdateItemAsync(item);

            if (!isUpdated)
            {
                return;
            }

            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave(object arg)
        {
            if (item == null)
            {
                return false;
            }

            return !String.IsNullOrWhiteSpace(URL) &&
                !String.IsNullOrWhiteSpace(Name) &&
                (item.Name != Name ||
                item.URL != URL);
        }

        private async void OnCancel(object obj)
        {
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadItemId(int itemId)
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
