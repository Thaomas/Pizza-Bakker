using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Kitchen;
using System.Collections.Generic;

namespace Pizza_Client.Commands.KitchenCommands
{
    public class PlaceOrderCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private KitchenViewModel _placeOrderViewModel => (KitchenViewModel)_navigationStore.CurrentViewModel;

        public PlaceOrderCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            Dictionary<int, List<string>> list = new();

            List<string> pizzaPollo = new();
            pizzaPollo.Add("pizza pollo");
            pizzaPollo.Add("kaas");
            pizzaPollo.Add("deeg");
            pizzaPollo.Add("uien");
            pizzaPollo.Add("kip");

            List<string> pizzamargarita = new();
            pizzamargarita.Add("pizza margarttia");
            pizzamargarita.Add("kaas");
            pizzamargarita.Add("uien");
            pizzamargarita.Add("deeg");
            pizzamargarita.Add("salami");

            list.Add(1, pizzaPollo);
            list.Add(2, pizzamargarita);

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<PlaceOrderRequestPacket>()
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderRequestPacket()
                {
                    pizzaOrder = list
                }
            }, PlaceOrderCallback);
        }

        private void PlaceOrderCallback(DataPacket obj)
        {
            PlaceOrderResponsePacket data = obj.GetData<PlaceOrderResponsePacket>();

            _placeOrderViewModel.AllOrders = data.orderList;
        }
    }
}