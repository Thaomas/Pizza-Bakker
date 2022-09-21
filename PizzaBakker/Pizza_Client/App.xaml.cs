using REI.Stores;
using REI.Util;
using REI.ViewModels;
using System.Threading;
using System.Windows;

namespace REI
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
