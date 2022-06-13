using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using BlackLion.QRStore.Views;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly IMessageService _messageService;
        private Item item;
        private int itemId;

        public Item Item
        {
            get => item;
            set => SetProperty(ref item, value);
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

        public Command ClickVisitButtonCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command EditItemCommand { get; }

        public ItemDetailViewModel()
        {
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            ClickVisitButtonCommand = new Command(OnClickVisitButton);
            DeleteItemCommand = new Command(OnDeleteItem);
            EditItemCommand = new Command(OnEditItem);
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                Item = await _dataStore.GetItemAsync(itemId);
                ItemId = Item.Id;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnDeleteItem()
        {
            bool shouldDelete = await _messageService.ShowAsync(
                "Do you want to delete this entry?",
                "After pressing \"Yes\" the entry will be erased and you'll be unable to recuperate it.",
                "Yes",
                "No"
            );

            if (!shouldDelete)
            {
                return;
            }

            var isDeleted = await _dataStore.DeleteItemAsync(itemId);

            if (!isDeleted)
            {
                await _messageService.ShowAsync("Oopps!", "We couldn't delete this entry due to an error.", "Ok");

                return;
            }

            await Shell.Current.GoToAsync("//ItemsPage");
        }

        private async void OnClickVisitButton()
        {
            try
            {
                await Browser.OpenAsync(item.URL, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                await _messageService.ShowAsync("Oopps!", "We couldn't open this webpage due to an error.", "Ok");
            }
        }

        private async void OnEditItem()
        {
            await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?id={Item.Id}");
        }
    }
}
