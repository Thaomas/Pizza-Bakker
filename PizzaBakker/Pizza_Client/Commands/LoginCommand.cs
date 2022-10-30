using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Login;
using System;
using System.Diagnostics;
using System.Windows;

namespace Employee_Client.Commands
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
            string type = parameter as string;
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            if (!connectionHandler.IsConnected || connectionHandler.ID == Guid.Empty)
            {
                Trace.WriteLine("No Connection to server.");
                return;
            }

            if (_loginViewModel.Username is null ||
                _loginViewModel.Password is null)
            {
                Trace.WriteLine("No password or username entered.");
                return;
            }
            uint username;
            if (!uint.TryParse(_loginViewModel.Username, out username))
            {
                return;
            }

            connectionHandler.SendData(new DataPacket<LoginPacket>()
            {
                type = PacketType.LOGIN,
                data = new LoginPacket()
                {
                    username = username,
                    password = _loginViewModel.Password,
                    clientType = type.Equals("Baker") ? ClientType.BAKER : ClientType.WAREHOUSE
                }
            }, LoginCallback); ;
        }

        public void LoginCallback(DataPacket packet)
        {
            LoginResponsePacket data = packet.GetData<LoginResponsePacket>();
            if (data.statusCode == StatusCode.ACCEPTED)
            {
                Trace.WriteLine(data.clientType);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    BaseViewModel viewModel = data.clientType.Equals(ClientType.BAKER) ? new KitchenViewModel(_navigationStore) : new WarehouseViewModel(_navigationStore);
                    _navigationStore.CurrentViewModel = viewModel;
                });
                return;
            }

            Trace.WriteLine("Wrong login info.");
        }
    }
}