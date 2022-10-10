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
            if (!uint.TryParse(_loginViewModel.Username, out _))
            {
                return;
            }

            connectionHandler.SendData(new DataPacket<LoginPacket>()
            {
                type = PacketType.LOGIN,
                data = new LoginPacket()
                {
                    username = uint.Parse(_loginViewModel.Username),
                    password = _loginViewModel.Password,
                    clientType = ClientType.BAKER
                }
            }, LoginCallback);
        }

        public void LoginCallback(DataPacket packet)
        {
            if (packet.GetData<LoginResponsePacket>().statusCode == StatusCode.ACCEPTED)
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _navigationStore.CurrentViewModel = new KitchenViewModel(_navigationStore);
                });

                return;
            }

            Trace.WriteLine("Wrong login info.");
        }
    }
}
