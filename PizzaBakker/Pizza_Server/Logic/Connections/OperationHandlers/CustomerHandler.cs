using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
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
        private List<string> pizzasInBasket = new();
        public CustomerHandler(Server server, Client client) : base(server, client)
        {
            _customer = Customer.Instance;
            OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                { PacketType.PLACE_ORDER, PlaceOrder},
                { PacketType.GET_PIZZA_LIST, GetPizzas},
                { PacketType.GET_CUSTOMER_ID, GetID },
                { PacketType.ADD_TO_BASKET, AddToBasket}
            };
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

            List<string> pizzaOrder = packet.GetData<PlaceOrderRequestPacket>().pizzaOrder;
            PizzaOrder _pizzaOrder = new();
            StatusCode _statusCode;
            
            _pizzaOrder.OrderId = Guid.NewGuid();
            _pizzaOrder.Status = OrderStatus.ORDERED;
            _kitchen.orderPizza(pizzaOrder);

            if (_kitchen._orderComplete)
            {
                _statusCode = StatusCode.OK;
                
                List<string> pizzas = new();
                foreach (var singlePizzaa in pizzaOrder) {
                    pizzas.Add(singlePizzaa);
                }

                _pizzaOrder.AllPizzas = pizzas;
                
                foreach (string pizza in _pizzaOrder.AllPizzas) {
                    Console.WriteLine("pizza: " + pizza);
                }

                _kitchen.AddOrder(_pizzaOrder);
              
            } else {
                _statusCode = StatusCode.BAD_REQUEST;
                Console.WriteLine("order gefaald");
            }
            
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
