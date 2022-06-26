using System.Collections.Generic;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;

namespace BlackLion.QRStore.Views
{
    public partial class ScanQRCodePage : ContentPage
    {
        public ScanQRCodePage()
        {
            InitializeComponent();

            var options = new MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<BarcodeFormat>() {
                BarcodeFormat.QR_CODE
            };
            QRScanner.Options = options;
        }

        private void OnFlashButtonClicked(object sender, System.EventArgs e)
        {
            QRScanner.IsTorchOn = !QRScanner.IsTorchOn;
        }
    }
}