using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;
using System;

namespace Employee_Client.Commands.WarehouseCommands
{
    public class ReloadListCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private DateTime _newestVersion;
        private WarehouseViewModel _warehouseViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;
        public ReloadListCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _newestVersion = DateTime.MinValue;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(new DataPacket<GetIngredientListRequestPacket>
            {
                type = PacketType.GET_INGREDIENT_LIST,
                data = new GetIngredientListRequestPacket()
                {
                    newest = _newestVersion
                }
            }, ReloadListCallBack); ;
        }

        private void ReloadListCallBack(DataPacket packet)
        {
            GetIngredientListResponsePacket data = packet.GetData<GetIngredientListResponsePacket>();

            if (!data.statusCode.Equals(StatusCode.OK)) return;

            _newestVersion = data.newest;
            _warehouseViewModel.AllIngredients = data.allItems;
        }
    }
}