using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;

namespace Customer_Client.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public LogoutCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            UserInfo.Delete();
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
    }
}
