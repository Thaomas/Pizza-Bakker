using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;

namespace Employee_Client.Commands.WarehouseCommands
{
    public class ReloadListCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private WarehouseViewModel _warehouseViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;
        public ReloadListCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(new DataPacket<GetListRequestPacket>
            {
                type = PacketType.GET_INGREDIENT_LIST,
                data = new GetListRequestPacket() { }
            }, ReloadListCallBack);
        }

        private void ReloadListCallBack(DataPacket packet)
        {
            GetIngredientListResponsePacket data = packet.GetData<GetIngredientListResponsePacket>();
            _warehouseViewModel.AllIngredients = data.allItems;
        }
    }
}