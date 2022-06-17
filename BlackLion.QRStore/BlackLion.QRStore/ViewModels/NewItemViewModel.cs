using BlackLion.QRStore.Helpers;
using BlackLion.QRStore.Localization;
using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
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
        public Command VisitNowCommand { get; }

        public NewItemViewModel()
        {
            _dataStore = DependencyService.Get<IDataStore<Item>>();
            _messageService = DependencyService.Get<IMessageService>();
            Title = NewItemPageResources.Page_Title;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(async () => await OnCancel());
            VisitNowCommand = new Command(async() => await OnVisitNow());
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            if (url == null)
            {
                return false;
            }

            IsValidURL = URLValidatorHelper.IsValidURL(URL);

            return !string.IsNullOrWhiteSpace(url) &&
                !string.IsNullOrWhiteSpace(name) &&
                isValidURL;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            URL = HttpUtility.UrlDecode(query["url"]);
        }

        private async Task OnCancel()
        {
            await Shell.Current.GoToAsync("//ItemsPage");
        }

        private async void OnSave()
        {
            var duplicatedItem = (await _dataStore.GetItemsAsync()).Find(item => item.URL == url);

            if (duplicatedItem != null)
            {
                await _messageService.ShowAsync(
                    NewItemPageResources.Save_Modal_Title,
                    NewItemPageResources.Save_Modal_Content,
                    NewItemPageResources.Save_Modal_Ok_Button
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

        private async Task OnVisitNow()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }).ContinueWith(async _ =>
                {
                    await Task.Delay(300);
                    await Shell.Current.GoToAsync("//ItemsPage");
                });
            }
            catch (Exception)
            {
                await _messageService.ShowAsync(
                    NewItemPageResources.Visit_Now_Modal_Title,
                    NewItemPageResources.Visit_Now_Modal_Content,
                    NewItemPageResources.Visit_Now_Modal_Ok_Option
                );
            }
        }
    }
}
