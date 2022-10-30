
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System;

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
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<PlaceOrderRequestPacket>()
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderRequestPacket()
                {
                    pizzaOrder = _homePageViewModel.PizzasInBasket
                }
            });
        }

        private void PlaceOrderCallback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}