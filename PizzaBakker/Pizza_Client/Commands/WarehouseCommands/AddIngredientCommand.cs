using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;

namespace Employee_Client.Commands.WarehouseCommands
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
            });
        }
    }
}