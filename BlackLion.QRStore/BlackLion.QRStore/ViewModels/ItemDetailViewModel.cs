using BlackLion.QRStore.Helpers;
using BlackLion.QRStore.Localization;
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
            Title = ItemDetailPageResources.Page_Title;
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
                ItemDetailPageResources.Delete_Item_Modal_Title,
                ItemDetailPageResources.Delete_Item_Modal_Content,
                ItemDetailPageResources.Delete_Item_Modal_Yes_Option,
                ItemDetailPageResources.Delete_Item_Modal_No_Option
            );

            if (!shouldDelete)
            {
                return;
            }

            var isDeleted = await _dataStore.DeleteItemAsync(itemId);

            if (!isDeleted)
            {
                await _messageService.ShowAsync(
                    ItemDetailPageResources.Delete_Item_Error_Modal_Title,
                    ItemDetailPageResources.Delete_Item_Error_Modal_Content,
                    ItemDetailPageResources.Delete_Item_Error_Modal_Ok_Option
                );

                return;
            }

            await Shell.Current.GoToAsync("//ItemsPage");
        }

        private async void OnClickVisitButton()
        {
            try
            {
                item.URL = URLHelper.NormalizeURL(item.URL);

                await Browser.OpenAsync(item.URL, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                await _messageService.ShowAsync(
                    ItemDetailPageResources.Visit_Link_Error_Modal_Title,
                    ItemDetailPageResources.Visit_Link_Error_Modal_Content,
                    ItemDetailPageResources.Visit_Link_Error_Modal_Ok_Option
                );
            }
        }

        private async void OnEditItem()
        {
            await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?id={Item.Id}");
        }
    }
}
