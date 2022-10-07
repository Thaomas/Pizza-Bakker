using System;
using System.Diagnostics;
using REI.Stores;
using REI.Util;
using REI.ViewModels;
using Shared;
using Shared.Login;
using Shared.Order;

namespace REI.Commands
{
    public class AddIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public AddIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ((WarehouseViewModel)_navigationStore.CurrentViewModel).Debug = "miauwww";
            
            /*ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(Callback, new DataPacket<AddIngredientPacket>()
            {
                type = PacketType.ADD_INGREDIENT,
                senderID = connectionHandler.ID,
                
            });*/
        }

        private void Callback(DataPacket packet)
        {
            ((WarehouseViewModel)_navigationStore.CurrentViewModel).Debug = "miauwww";
        }
    }
}