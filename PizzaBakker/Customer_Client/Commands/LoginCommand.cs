using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Customer_Client.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private LoginViewModel _loginViewModel => (LoginViewModel)_navigationStore.CurrentViewModel;

        public LoginCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            Trace.WriteLine("AAAAAA " + _loginViewModel.Name);
            if (_loginViewModel.Name.Equals(""))
            {
                new Task(async () =>
                {
                    string old = _loginViewModel.Label;
                    _loginViewModel.Label = "Enter a Username";
                    await Task.Delay(2000);
                    _loginViewModel.Label = old;
                }).Start();
                return;
            }

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            while (!connectionHandler.IsConnected) ;
            connectionHandler.SendData(new DataPacket<GetCustomerIDPacket>()
            { type = PacketType.GET_CUSTOMER_ID }, LoginCallback);
        }

        public void LoginCallback(DataPacket packet)
        {
            GetCustomerIDResponsePacket data = packet.GetData<GetCustomerIDResponsePacket>();
            new UserInfo(data.customerID, _loginViewModel.Name);
            UserInfo.Save();
            _loginViewModel.ChangeViewModel();
        }


    }
}
