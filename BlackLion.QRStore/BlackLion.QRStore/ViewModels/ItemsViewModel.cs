using BlackLion.QRStore.Localization;
using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using BlackLion.QRStore.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly IMessageService _messageService;
        private Item _selectedItem;
        private bool isSearchBarVisible;
        private string searchBarText;
        private string toolbarSearchIcon;
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command ScanQRCodeCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command SearchTextChangeCommand { get; }
        public Command<Item> SwipeDeleteCommand { get; }
        public Command ToggleSearchBarCommand { get; }
        

        public bool IsSearchBarVisible
        {
            get => isSearchBarVisible;
            set => SetProperty(ref isSearchBarVisible, value);
        }

        public string SearchBarText
        {
            get => searchBarText;
            set
            {
                SetProperty(ref searchBarText, value);
            }
        }

        public string ToolbarSearchIcon
        {
            get => toolbarSearchIcon;
            set => SetProperty(ref toolbarSearchIcon, value);
        }

        public ItemsViewModel()
        {
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            Title = ItemsPageResources.Page_Title;
            Items = new ObservableCollection<Item>();
            IsSearchBarVisible = false;
            SearchBarText = string.Empty;
            ToolbarSearchIcon = "icon_search.png";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            ScanQRCodeCommand = new Command(OnScanButtonClicked);
            SwipeDeleteCommand = new Command<Item>(async (item) => await OnItemSwept(item));
            ToggleSearchBarCommand = new Command(OnSearchButtonClicked);
            SearchTextChangeCommand = new Command(OnSearchTextChange);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                
                var items = await _dataStore.GetItemsAsync(true);
                
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnScanButtonClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ScanQRCodePage));
        }

        private async void OnSearchTextChange(object obj)
        {
            Items.Clear();

            if (string.IsNullOrEmpty(obj as string))
            {
                var data = await _dataStore.GetItemsAsync(true);

                data.ForEach(item => Items.Add(item));
            }
            else
            {
                var data = await _dataStore.FindAllByPredicateAsync(i => i.Name.ToLower().Contains((obj as string).ToLower()));

                data.ForEach(item => Items.Add(item));
            }
        }

        private async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        private async Task OnItemSwept(Item item)
        {
            bool shouldDelete = await _messageService.ShowAsync(
                ItemsPageResources.Modal_Item_Swept_Title,
                ItemsPageResources.Modal_Item_Swept_Content,
                ItemsPageResources.Modal_Item_Swept_Yes_Option,
                ItemsPageResources.Modal_Item_Swept_No_Option
            );

            if (!shouldDelete)
            {
                return;
            }

            var isDeleted = await _dataStore.DeleteItemAsync(item.Id);

            if (!isDeleted)
            {
                await _messageService.ShowAsync(
                    ItemsPageResources.Modal_Item_Swept_Error_Title,
                    ItemsPageResources.Modal_Item_Swept_Error_Content,
                    ItemsPageResources.Modal_Item_Swept_Error_Yes_Option
                );

                return;
            }

            Items.Remove(item);
        }

        private void OnSearchButtonClicked(object obj)
        {
            if (toolbarSearchIcon == "icon_search.png")
            {
                ToolbarSearchIcon = "icon_close.png";
            }
            else
            {
                SearchBarText = string.Empty;
                ToolbarSearchIcon = "icon_search.png";
            }

            IsSearchBarVisible = !IsSearchBarVisible;
        }
    }
}