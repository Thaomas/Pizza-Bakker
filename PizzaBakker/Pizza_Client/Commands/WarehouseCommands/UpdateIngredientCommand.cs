using System;
using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Kitchen;
using Shared.Warehouse;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class UpdateIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private WarehouseViewModel _warehouseViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;

        public UpdateIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(new DataPacket<UpdateIngredientRequestPacket>()
            {
                type = PacketType.UPDATE_INGREDIENT,
                data = new UpdateIngredientRequestPacket()
                {
                    count = Convert.ToInt32(_warehouseViewModel.IngredientAmount),
                    name = _warehouseViewModel.IngredientName,
                    price = Convert.ToInt32(_warehouseViewModel.IngredientPrice)
                }
            }, DeleteIngredientCallback);
        }

        private void DeleteIngredientCallback(DataPacket obj)
        {
            DeleteIngredientResponsePacket data = obj.GetData<DeleteIngredientResponsePacket>();
            _warehouseViewModel.AllIngredients = data.warehouseList;
        }
    }
}