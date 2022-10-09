using System;
using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Warehouse;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class AddIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private WarehouseViewModel _addIngredientViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;

        public AddIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        { 
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<AddIngredientRequestPacket>()
            {
                type = PacketType.ADD_INGREDIENT,
                data = new AddIngredientRequestPacket()
                {
                    ingredient = new WarehouseItem()
                    {
                        Count = _addIngredientViewModel.NewIngredientAmount,
                        Ingredient = new Ingredient()
                        {
                            Name = _addIngredientViewModel.NewIngredientName,
                            Price = _addIngredientViewModel.NewIngredientPrice
                        }
                    } 
                }
            }, AddIngredientCallback);
        }

        private void AddIngredientCallback(DataPacket obj)
        {
            AddIngredientResponsePacket data = obj.GetData<AddIngredientResponsePacket>();
            _addIngredientViewModel.AllIngredients = data.warehouseList;
        }
    }
}