using BlackLion.QRStore.Views;
using System;
using Xamarin.Forms;

namespace BlackLion.QRStore
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(ScanQRCodePage), typeof(ScanQRCodePage));
            Routing.RegisterRoute(nameof(EditItemPage), typeof(EditItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }
    }
}
