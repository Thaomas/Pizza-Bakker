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
        public override void Execute(object parameter)
        {
            PizzaOrder order = parameter as PizzaOrder;
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<ChangeStatusOrderRequestPacket>()
            {
                type = PacketType.CHANGE_STATUS,
                data = new ChangeStatusOrderRequestPacket()
                {
                    pizzaOrderId = order.OrderId,
                    pizzaOrderStatus = order.Status
                }
            });
        }
    }
}