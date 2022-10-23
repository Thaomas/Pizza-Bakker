using System.Threading;
using System.Windows;
using Customer_Client.Stores;
using Customer_Client.Util;
using Customer_Client.ViewModels;

namespace Customer_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            new Thread(ConnectionHandler.GetInstance().ConnectToServer).Start();

            _navigationStore.CurrentViewModel = new HomePageViewModel(_navigationStore);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}