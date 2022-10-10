using System;
using System.Collections.Generic;
using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Kitchen;
using Shared.Warehouse;

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
            //5  50  19  19 28
            List<string> pizzaPollo = new();
            pizzaPollo.Add("pizza pollo");
            pizzaPollo.Add("kaas");
            pizzaPollo.Add("deeg");
            pizzaPollo.Add("uien");
            pizzaPollo.Add("kip");
            
            List<string> pizzamargarita = new();
            pizzamargarita.Add("pizza margarttia");
            pizzamargarita.Add("salami");
            pizzamargarita.Add("kaas");
            pizzamargarita.Add("deeg");
            
            
            list.Add(1,pizzaPollo);
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
            //AddIngredientResponsePacket data = obj.GetData<AddIngredientResponsePacket>();
            //_addIngredientViewModel.AllIngredients = data.warehouseList;
        }
    }
}