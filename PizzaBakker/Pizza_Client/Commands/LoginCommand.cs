using Newtonsoft.Json.Linq;
using REI.Stores;
using REI.Util;
using REI.ViewModels;
using Shared;
using Shared.Login;
using System;
using System.Diagnostics;
using System.Windows;

namespace REI.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

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

            if (((LoginViewModel)_navigationStore.CurrentViewModel).Username is null ||
                ((LoginViewModel)_navigationStore.CurrentViewModel).Password is null)
            {
                Trace.WriteLine("No password or username entered.");
                return;
            }
            if (!uint.TryParse(((LoginViewModel)_navigationStore.CurrentViewModel).Username, out _))
            {
                return;
            }

            connectionHandler.SendData(LoginCallback, new DataPacket<LoginPacket>()
            {
                type = PacketType.LOGIN,
                senderID = (Guid)connectionHandler.ID,
                data = new LoginPacket()
                {
                    username = uint.Parse(((LoginViewModel)_navigationStore.CurrentViewModel).Username),
                    password = ((LoginViewModel)_navigationStore.CurrentViewModel).Password
                }
            });
        }

        public void LoginCallback(DataPacket packet)
        {
            if (packet.GetData<LoginResponsePacket>().statusCode == StatusCode.ACCEPTED)
            {
                ConnectionHandler.GetInstance().ID = packet.senderID;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _navigationStore.CurrentViewModel = new WarehouseViewModel(_navigationStore);
                });

                return;
            }

            Trace.WriteLine("Wrong login info.");
        }
    }
}
