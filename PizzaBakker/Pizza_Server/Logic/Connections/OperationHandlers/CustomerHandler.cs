using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using Shared.Packet.Kitchen;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class CustomerHandler : OpHndlrAbstract
    {
        private Kitchen _kitchen;
        private Customer _customer;

        public CustomerHandler(Server server, Client client) : base(server, client)
        {
            _customer = Customer.Instance;
            _kitchen = Kitchen.Instance;
            OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                { PacketType.PLACE_ORDER, PlaceOrder},
                { PacketType.GET_PIZZA_LIST, GetPizzas},
                { PacketType.GET_CUSTOMER_ID, GetID },
                { PacketType.ADD_TO_BASKET, AddToBasket},
                { PacketType.GET_ORDER_HISTORY, GetOrderHistory }
            };
        }

        private void GetOrderHistory(DataPacket obj)
        {
            GetOrderHistoryPacket data = obj.GetData<GetOrderHistoryPacket>();
            _client.SendData(new DataPacket<GetOrderHistoryResponsePacket>
            {
                type = PacketType.GET_ORDER_HISTORY,
                data = new GetOrderHistoryResponsePacket()
                {
                    orderHistory = Kitchen.Instance.GetSpecificOrders(data.customerID)
                }
            });
        }

        private void GetID(DataPacket obj)
        {
            _client.SendData(new DataPacket<GetCustomerIDResponsePacket>
            {
                type = PacketType.GET_CUSTOMER_ID,
                data = new GetCustomerIDResponsePacket()
                {
                    customerID = Guid.NewGuid()
                }
            });
        }

        private void AddToBasket(DataPacket obj)
        {
            string selectedPizza = obj.GetData<AddToBasketRequestPacket>().pizzaName;

            pizzasInBasket.Add(selectedPizza);

            Client client = _server.IdToClient[obj.senderID];
            client.SendData(new DataPacket<AddToBasketResponsePacket>
            {
                type = PacketType.GET_PIZZA_LIST,
                data = new AddToBasketResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    pizzas = pizzasInBasket
                }
            });

        }

        private void GetPizzas(DataPacket obj)
        {
            Console.WriteLine(_client._guid);
            Dictionary<string, List<string>> pizzas = new Dictionary<string, List<string>>();

            _client.SendData(new DataPacket<GetListResponsePacket>
            {
                type = PacketType.GET_PIZZA_LIST,
                data = new GetListResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    pizzas = Customer.Instance.getPizzas()
                }
            });
        }

        public void PlaceOrder(DataPacket packet)
        {
            PlaceOrderRequestPacket data = packet.GetData<PlaceOrderRequestPacket>();
            List<string> newOrder = data.pizzaOrder;
            bool complete = _kitchen.orderPizzas(newOrder, data.customerID);

            StatusCode _statusCode = complete ? StatusCode.OK : StatusCode.BAD_REQUEST;

            _client.SendData(new DataPacket<PlaceOrderResponsePacket>
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderResponsePacket()
                {
                    statusCode = _statusCode
                }
            });
        }


    }
}
