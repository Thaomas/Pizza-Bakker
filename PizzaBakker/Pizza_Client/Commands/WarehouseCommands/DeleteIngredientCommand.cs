using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;

namespace Employee_Client.Commands.WarehouseCommands
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
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<DeleteIngredientRequestPacket>()
            {
                type = PacketType.DELETE_INGREDIENT,
                data = new DeleteIngredientRequestPacket()
                {
                    singleIngredient = _warehouseViewModel.SelectedIngredient
                }
            });
        }
    }
}