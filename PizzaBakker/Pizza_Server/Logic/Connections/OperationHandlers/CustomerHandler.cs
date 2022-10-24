using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Packet.Customer_Client;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class CustomerHandler : OpHndlrAbstract
    {
        private Kitchen _kitchen;
        private Customer _customer;
        public CustomerHandler(Server server)
        {
            _server = server;
            _kitchen = Kitchen.Instance;
            _customer = Customer.Instance;
            OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                { PacketType.PLACE_ORDER, PlaceOrder},
                { PacketType.GET_PIZZA_LIST, GetPizzas}
            };
        }

        private void GetPizzas(DataPacket obj)
        {
            Client client = _server.IdToClient[obj.senderID];
            client.SendData(new DataPacket<GetListResponsePacket>
            {
                type = PacketType.GET_ORDER_LIST,
                data = new GetListResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    pizzas = _customer.pizzas
                }
            });
        }

        public void PlaceOrder(DataPacket packet)
        {

            Dictionary<int, List<string>> pizzaOrder = packet.GetData<PlaceOrderRequestPacket>().pizzaOrder;
            PizzaOrder _pizzaOrder = new();

            _pizzaOrder.OrderId = Guid.NewGuid();
            _pizzaOrder.Status = OrderStatus.ORDERED;
            _kitchen.orderPizza(pizzaOrder);

            if (_kitchen._orderComplete)
            {
                List<string> pizza = new();
                foreach (var singlePizzaa in pizzaOrder.Values)
                {
                    pizza.Add(singlePizzaa[0]);
                }

                _pizzaOrder.AllPizzas = pizza;

                Console.WriteLine("order is gecompleted, het wordt nu omgezet naar een PIZZA_ORDER OBJECT");

            }
            else
            {
                Console.WriteLine("order gefaald");
            }

            Console.WriteLine("ordernummer is: " + _pizzaOrder.OrderId);
            foreach (string pizza in _pizzaOrder.AllPizzas)
            {
                Console.WriteLine("pizza: " + pizza);
            }

            Kitchen.Instance.AddOrder(_pizzaOrder);
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<PlaceOrderResponsePacket>
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderResponsePacket()
                {
                    statusCode = StatusCode.OK,
                }
            });
        }
    }
}
