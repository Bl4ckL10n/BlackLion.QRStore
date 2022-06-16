using BlackLion.QRStore.Views;
using System.Web;
using Xamarin.Forms;
using ZXing;

namespace BlackLion.QRStore.ViewModels
{
    public class ScanQRCodeViewModel : BaseViewModel
    {
        private bool isScanning;

        public bool IsScanning
        {
            get => isScanning;
            set => SetProperty(ref isScanning, value);
        }

        public Command ScanResultCommand { get; }

        public ScanQRCodeViewModel()
        {
            Title = "Scan";
            IsScanning = true;
            ScanResultCommand = new Command(OnScanResult);
        }

        private void OnScanResult(object obj)
        {
            IsScanning = false;
            var scappedURL = HttpUtility.UrlEncode((obj as Result).Text);

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?url={scappedURL}");
            });
        }
    }
}
