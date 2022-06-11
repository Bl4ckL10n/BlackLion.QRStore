using BlackLion.QRStore.Models;
using BlackLion.QRStore.ViewModels;
using Xamarin.Forms;

namespace BlackLion.QRStore.Views
{
    public partial class NewItemPage : ContentPage
    {
        //public string Domain { get; set; }
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
        }
    }
}