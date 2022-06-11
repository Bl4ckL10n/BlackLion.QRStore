using System.Threading.Tasks;

namespace BlackLion.QRStore.Services
{
    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> ShowAsync(string title, string message, string confirmText, string cancelText)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, confirmText, cancelText);
        }
    }
}
