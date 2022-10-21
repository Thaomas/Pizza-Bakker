using Employee_Client.Commands;
using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System;

namespace Employee_Client.Commands.KitchenCommands
{
    public class CheckOrderListCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private KitchenViewModel _kitchenViewModel => (KitchenViewModel)_navigationStore.CurrentViewModel;

        private DateTime _newestOrderList;
        public CheckOrderListCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _newestOrderList = DateTime.MinValue;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<CheckOrderChangesPacket>()
            {
                type = PacketType.GET_ORDER_LIST,
                data = new CheckOrderChangesPacket()
                {
                    newest = _newestOrderList
                }
            }, CheckOrderCallback);
        }

        public void CheckOrderCallback(DataPacket packet)
        {
            CheckOrderChangesResponsePacket data = packet.GetData<CheckOrderChangesResponsePacket>();
            if (data.statusCode.Equals(StatusCode.OK))
            {
                _kitchenViewModel.AllOrders = data.orders;
                _newestOrderList = data.newest;
            }

        }
    }
}
