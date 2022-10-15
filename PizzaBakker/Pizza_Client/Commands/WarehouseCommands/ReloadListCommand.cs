using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Warehouse;

namespace Pizza_Client.Commands.WarehouseCommands
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