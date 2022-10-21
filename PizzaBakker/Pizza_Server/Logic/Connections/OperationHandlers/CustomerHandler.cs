using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class CustomerHandler : OpHndlrAbstract
    {
        private Kitchen _kitchen;
        public CustomerHandler(Server server)
        {
            _server = server;
            _kitchen = Kitchen.Instance;
            this.OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                { PacketType.PLACE_ORDER, PlaceOrder}
            };
        }

        public void PlaceOrder(DataPacket packet)
        {

            Dictionary<int, List<string>> pizzaOrder = packet.GetData<PlaceOrderRequestPacket>().pizzaOrder;
            PizzaOrder _pizzaOrder = new();

            _pizzaOrder.OrderId = (uint)new Random().Next(0, 1000);
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

            Kitchen.Instance.AllOrders.Add(_pizzaOrder);
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<PlaceOrderResponsePacket>
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    orderList = Kitchen.Instance.AllOrders.ToList()
                }
            });
        }
    }
}
