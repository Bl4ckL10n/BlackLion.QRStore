using BlackLion.QRStore.Models;
using BlackLion.QRStore.Services;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlackLion.QRStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton<IDataStore<Item>>(
                new SQLiteDataStore(Path.Combine(FileSystem.AppDataDirectory, "Items.db3"))
            );
            DependencyService.Register<IMessageService, MessageService>();
            
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
