using BlackLion.QRStore.Views;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;

namespace BlackLion.QRStore.ViewModels
{
    public class ScanQRCodeViewModel : BaseViewModel
    {
        private bool isScanning;
        private MobileBarcodeScanningOptions options;

        public bool IsScanning
        {
            get => isScanning;
            set => SetProperty(ref isScanning, value);
        }
        public MobileBarcodeScanningOptions Options
        {
            get => options;
            set => SetProperty(ref options, value);
        }

        public Command OnScanResultCommand { get; }

        public ScanQRCodeViewModel()
        {
            Title = "Scan";
            var Options = new MobileBarcodeScanningOptions();
            Options.PossibleFormats = new List<BarcodeFormat>() {
                BarcodeFormat.QR_CODE
            };
            IsScanning = true;
            OnScanResultCommand = new Command(OnScanResult);
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
