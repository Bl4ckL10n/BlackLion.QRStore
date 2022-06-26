using BlackLion.QRStore.Localization;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlackLion.QRStore.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Launcher.OpenAsync(new Uri(url));
        });

        public AboutViewModel()
        {
            Title = AboutPageResources.Page_Title;
        }
    }
}