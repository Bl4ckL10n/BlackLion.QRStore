using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlackLion.QRStore.Views
{
    public partial class EditItemPage : ContentPage
    {
        public EditItemPage()
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

            nameEntry.CursorPosition = nameEntry.Text.Length;
        }
    }
}