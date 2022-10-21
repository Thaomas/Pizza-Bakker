using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class BakerHandler : OpHndlrAbstract
    {
        Kitchen kitchen = Kitchen.Instance;
        public BakerHandler(Server server)
        {
            _server = server;
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
            Client client = _server.IdToClient[packet.senderID];

            if (data.newest < kitchen.NewestOrderDateTime)
            {
                client.SendData(new DataPacket<CheckOrderChangesResponsePacket>
                {
                    type = packet.type,
                    data = new CheckOrderChangesResponsePacket()
                    {
                        statusCode = StatusCode.OK,
                        newest = kitchen.NewestOrderDateTime,
                        orders = kitchen.AllOrders.ToList()
                    }
                });
            }
            else
            {
                client.SendData(new DataPacket<CheckOrderChangesResponsePacket>
                {
                    type = packet.type,
                    data = new CheckOrderChangesResponsePacket()
                    {
                        newest = kitchen.NewestOrderDateTime,
                        statusCode = StatusCode.BAD_REQUEST
                    }
                });

            }
        }
    }
}
