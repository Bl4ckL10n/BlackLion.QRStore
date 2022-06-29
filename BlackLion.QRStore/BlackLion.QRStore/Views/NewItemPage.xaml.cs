using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlackLion.QRStore.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            while (!nameEntry.Focus())
            {
                await Task.Delay(50);
            }
        }
    }
}