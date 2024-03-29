﻿using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Login;
using System;
using System.Diagnostics;
using System.Windows;

namespace Pizza_Client.Commands
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
                    clientType = (type.Equals("Baker")) ? ClientType.BAKER : ClientType.WAREHOUSE
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
                    BaseViewModel viewModel = (data.clientType.Equals(ClientType.BAKER)) ? new KitchenViewModel(_navigationStore) : new WarehouseViewModel(_navigationStore);
                    _navigationStore.CurrentViewModel = viewModel;
                });
                return;
            }

            Trace.WriteLine("Wrong login info.");
        }
    }
}
