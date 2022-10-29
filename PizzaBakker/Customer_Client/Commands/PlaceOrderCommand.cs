
using Shared;
using System.Collections.Generic;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System;
using Customer_Client.Stores;
using Customer_Client.Util;
using Customer_Client.ViewModels;

namespace Customer_Client.Commands
{
    public class PlaceOrderCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private HomePageViewModel _homePageViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;

        public PlaceOrderCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            List<string> list = new();

                           
            list.Add("Pizza Pollo");
            list.Add("Pizza Pollo");
            list.Add("Pizza Pollo");
            list.Add("Pizza Pepperoni");
            list.Add("Pizza Salami");
           // list.Add("pizza Margharita");

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<PlaceOrderRequestPacket>()
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderRequestPacket()
                {
                    pizzaOrder = list
                }
            });
        }

        private void PlaceOrderCallback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}