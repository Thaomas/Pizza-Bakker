using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;
using System;

namespace Employee_Client.Commands.WarehouseCommands
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
                    ingredientID = _warehouseViewModel.SelectedIngredient.Ingredient.Id,
                    count = Convert.ToUInt32(_warehouseViewModel.IngredientAmount),
                    name = _warehouseViewModel.IngredientName,
                    price = Convert.ToDecimal(_warehouseViewModel.IngredientPrice)
                }
            });
        }
    }
}