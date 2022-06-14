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
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command ScanQRCodeCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command<Item> SwipeDeleteCommand { get; }

        public ItemsViewModel()
        {
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            ScanQRCodeCommand = new Command(OnScanButtonClicked);
            SwipeDeleteCommand = new Command<Item>(async (item) => await OnItemSwept(item));
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
        
        private async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        private async Task OnItemSwept(Item item)
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

            var isDeleted = await _dataStore.DeleteItemAsync(item.Id);

            if (!isDeleted)
            {
                await _messageService.ShowAsync("Oopps!", "We couldn't delete this entry due to an error.", "Ok");

                return;
            }

            Items.Remove(item);
        }
    }
}