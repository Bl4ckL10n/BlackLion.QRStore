using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlackLion.QRStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQRCodePage : ContentPage
    {
        public ScanQRCodePage()
        {
            InitializeComponent();
        }

        private void OnFlashButtonClicked(object sender, System.EventArgs e)
        {
            QRScanner.IsTorchOn = !QRScanner.IsTorchOn;
        }
    }
}