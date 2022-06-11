using BlackLion.QRStore.Services;
using BlackLion.QRStore.Views;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlackLion.QRStore
{
    public partial class App : Application
    {
        static SQLiteDataStore database;

        public static SQLiteDataStore Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLiteDataStore(Path.Combine(FileSystem.AppDataDirectory, "Items.db3"));
                }

                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<SQLiteDataStore>();
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
