using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Order;
using System;
using System.Diagnostics;
using Shared.Warehouse;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class DeleteIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private WarehouseViewModel _warehouseViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;

        public DeleteIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            Trace.WriteLine("d");
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<DeleteIngredientRequestPacket>()
            {
                type = PacketType.DELETE_INGREDIENT,
                senderID = connectionHandler.ID,
                data = new DeleteIngredientRequestPacket()
                {
                    message = "hoiii"
                }
            }, DeleteIngredientCallback);
        }

        private void DeleteIngredientCallback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}