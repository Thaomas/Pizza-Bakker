using System;
using System.Diagnostics;
using Customer_Client.Stores;
using Customer_Client.Util;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using Shared.Packet.Login;

namespace Customer_Client.Commands;

public class InitCommand : CommandBase
{
     private readonly NavigationStore _navigationStore;
        private HomePageViewModel _mainViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;
        
        public InitCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            while (!connectionHandler.IsConnected) ;
            connectionHandler.SendData(new DataPacket<GetListRequestPacket>()
            {
                type = PacketType.GET_PIZZA_LIST,
                data = new GetListRequestPacket() { }
            }, LoginCallback); 
        }

        public void LoginCallback(DataPacket packet)
        {
            GetListResponsePacket data = packet.GetData<GetListResponsePacket>();

            _mainViewModel.AllPizzas = data.pizzas;
        }
    }
