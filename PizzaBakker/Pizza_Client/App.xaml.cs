using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using System.Threading;
using System.Windows;

namespace Employee_Client
{
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

            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
