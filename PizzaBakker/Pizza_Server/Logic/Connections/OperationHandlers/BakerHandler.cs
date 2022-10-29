using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class BakerHandler : OpHndlrAbstract
    {
        Kitchen kitchen = Kitchen.Instance;
        public BakerHandler(Server server, Client client) : base(server, client)
        {
            this.OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                {PacketType.CHANGE_STATUS, ChangeOrderStatus},
                {PacketType.GET_ORDER_LIST, GetList }
            };
        }

        private void ChangeOrderStatus(DataPacket obj)
        {
            ChangeStatusOrderRequestPacket pizza = obj.GetData<ChangeStatusOrderRequestPacket>();

            kitchen.ChangeOrderStatus(pizza.pizzaOrderId, pizza.pizzaOrderStatus);
        }

        public void GetList(DataPacket packet)
        {
            CheckOrderChangesPacket data = packet.GetData<CheckOrderChangesPacket>();
            List<PizzaOrder> allOrders = null;
            StatusCode code = StatusCode.BAD_REQUEST;

            if (data.newest < kitchen.NewestOrderDateTime)
            {
                kitchen.GetPizzaOrders(out allOrders);
                code = StatusCode.OK;
            }

            _client.SendData(new DataPacket<CheckOrderChangesResponsePacket>
            {
                type = packet.type,
                data = new CheckOrderChangesResponsePacket()
                {
                    statusCode = code,
                    newest = kitchen.NewestOrderDateTime,
                    orders = allOrders
                }
            });
        }
    }
}
