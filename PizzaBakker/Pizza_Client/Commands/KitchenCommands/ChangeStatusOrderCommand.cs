using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;

namespace Employee_Client.Commands.KitchenCommands
{
    public class ChangeStatusOrderCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private KitchenViewModel _placeOrderViewModel => (KitchenViewModel)_navigationStore.CurrentViewModel;

        public ChangeStatusOrderCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<ChangeStatusOrderRequestPacket>()
            {
                type = PacketType.CHANGE_STATUS,
                data = new ChangeStatusOrderRequestPacket()
                {
                    pizzaOrderId = ((PizzaOrder)parameter).OrderId,
                    pizzaOrderStatus = ((PizzaOrder)parameter).Status
                }
            });
        }
    }
}