using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Kitchen;
using System;

namespace Pizza_Client.Commands.KitchenCommands
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
