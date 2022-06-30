using BlackLion.QRStore.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BlackLion.QRStore
{
    public partial class AppShell : Shell
    {
        private List<string> _history;
        private string _action = "DEFAULT";

        public AppShell()
        {
            _history = new List<string>() { };
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

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Current != null)
            {
                if (args.Source == ShellNavigationSource.ShellItemChanged && _action == "DEFAULT")
                {
                    _history.Add(args.Current.Location.OriginalString);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (_history.Count > 0)
            {
                var lastRoute = _history[_history.Count - 1];
                _action = "GOING_BACK";

                _history.RemoveAt(_history.Count - 1);
                Current.GoToAsync(lastRoute);

                return true;
            }

            _action = "DEFAULT";

            return base.OnBackButtonPressed();
        }

    }
}
