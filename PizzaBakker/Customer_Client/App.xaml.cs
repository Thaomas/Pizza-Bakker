using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;
using System.Threading;
using System.Windows;

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
            new Thread(ConnectionHandler.GetInstance().ConnectToServer).Start();
        }

        protected override void OnStartup(StartupEventArgs e)
        {


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