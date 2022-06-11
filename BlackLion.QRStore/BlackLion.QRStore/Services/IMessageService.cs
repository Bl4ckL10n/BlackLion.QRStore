using System.Threading.Tasks;

namespace BlackLion.QRStore.Services
{
    internal interface IMessageService
    {
        Task ShowAsync(string title, string message, string cancel);
        Task<bool> ShowAsync(string title, string message, string confirmText, string cancelText);
    }
}
