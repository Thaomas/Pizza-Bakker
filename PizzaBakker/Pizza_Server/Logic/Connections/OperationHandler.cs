using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Logic.WarehouseNS;
using Pizza_Server.Main;
using Shared;
using Shared.Kitchen;
using Shared.Login;
using Shared.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic.Connections
{
    class OperationHandler
    {
        private readonly Server _server;
        private readonly Dictionary<PacketType, Action<DataPacket>> _customerOperationHandlers;
        private readonly Dictionary<PacketType, Action<DataPacket>> _bakerOperationHandlers;
        private readonly Dictionary<PacketType, Action<DataPacket>> _warehouseOperationHandlers;
        private Kitchen kitchen = Kitchen.GetInstance();

        public OperationHandler(Server viewModel)
        {
            _server = viewModel;
            _bakerOperationHandlers = new Dictionary<PacketType, Action<DataPacket>>
            {
            };

            _warehouseOperationHandlers = new Dictionary<PacketType, Action<DataPacket>>
            {
                { PacketType.ADD_INGREDIENT, AddIngredient},
                { PacketType.GET_LIST, GetList},
                { PacketType.DELETE_INGREDIENT, DeleteIngredient},
                { PacketType.PLACE_ORDER, PlaceOrder},
                { PacketType.CHANGE_STATUS, ChangeOrderStatus}

            };

            _customerOperationHandlers = new();
        }

        private void ChangeOrderStatus(DataPacket obj)
        {
            Console.WriteLine("operation changestatuss");
            Console.WriteLine(obj.GetData<ChangeStatusOrderRequestPacket>().pizzaOrder);
            
        }

        public void HandleDataCallback(DataPacket packet, Client client)
        {
            if (packet.type == PacketType.AUTHENTICATION)
            {
                AuthenticationResponsePacket authResponsePacket = packet.GetData<AuthenticationResponsePacket>();
                Action<DataPacket, Client> callback;
                switch (authResponsePacket.clientType)
                {
                    case ClientType.CUSTOMER:
                        callback = (DataPacket p, Client c) => _customerOperationHandlers[p.type](packet);
                        break;
                    case ClientType.EMPLOYEE:
                        callback = Authenticate;
                        break;
                    default:
                        callback = (DataPacket p, Client c) => { };
                        break;
                }
                client.Callback = callback;
                client.ClientType = authResponsePacket.clientType;
            }
        }


        public void Authenticate(DataPacket packet, Client client)
        {
            if (packet.type != PacketType.LOGIN || client.ClientType == ClientType.CUSTOMER)
            {
                client.SendData(new DataPacket<ErrorPacket>
                {
                    type = PacketType.ERROR,
                    data = new ErrorPacket
                    {
                        statusCode = (client.ClientType == ClientType.CUSTOMER) ? StatusCode.FORBIDDEN : StatusCode.BAD_REQUEST
                    }
                });
            }

            LoginPacket loginPacket = packet.GetData<LoginPacket>();
            uint id = loginPacket.username;
            Guid authId = packet.senderID;

            // Wrong login info.
            if (!_server.IdToEmployee.ContainsKey(id) ||
                _server.IdToEmployee[id].Password != loginPacket.password)
            {
                _server.IdToClient[authId].SendData(new DataPacket<LoginResponsePacket>
                {
                    type = PacketType.LOGIN,
                    data = new LoginResponsePacket()
                    {
                        statusCode = StatusCode.NOT_FOUND
                    }
                });
                return;
            }

            Employee employee = _server.IdToEmployee[id];
            Dictionary<PacketType, Action<DataPacket>> oppHandler = (client.ClientType == ClientType.BAKER) ? _bakerOperationHandlers : _warehouseOperationHandlers;


            client.Callback = (DataPacket p, Client c) => oppHandler[p.type](p);
            // Let the client know that it can log in. 
            client.SendData(new DataPacket<LoginResponsePacket>
            {
                type = PacketType.LOGIN,
                data = new LoginResponsePacket()
                {
                    statusCode = StatusCode.ACCEPTED
                }
            });

            _server.Log = $"Employee: {employee.WorkId}, Logged in as a {client.ClientType}";
        }

        public void AddIngredient(DataPacket packet)
        {
            AddIngredientRequestPacket addPacket = packet.GetData<AddIngredientRequestPacket>();

            uint id = Warehouse.GetInstance()._ingredients.Keys.Max();
            string name = addPacket.ingredient.Ingredient.Name;

            try
            {
                if (Warehouse.GetInstance()._ingredients.Values.All(v => v.Ingredient.Name != name))
                {
                    if (Warehouse.GetInstance()._ingredients.TryGetValue(id, out WarehouseItem dd))
                    {
                        uint total = id + 1;
                        addPacket.ingredient.Ingredient.Id = total;
                        Warehouse.GetInstance()._ingredients.Add(total, addPacket.ingredient);
                    }
                    else
                    {
                        addPacket.ingredient.Ingredient.Id = 1;
                        Warehouse.GetInstance()._ingredients.Add(1, addPacket.ingredient);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<AddIngredientResponsePacket>
            {
                type = PacketType.ADD_INGREDIENT,
                data = new AddIngredientResponsePacket()
                {
                    warehouseList = Warehouse.GetInstance()._ingredients.Values.ToList(),
                    statusCode = StatusCode.OK
                }
            });
        }


        public void DeleteIngredient(DataPacket packet)
        {
            DeleteIngredientRequestPacket deletePacket = packet.GetData<DeleteIngredientRequestPacket>();

            Warehouse.GetInstance()._ingredients.Remove(deletePacket.ID);

            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<DeleteIngredientResponsePacket>
            {

                type = PacketType.DELETE_INGREDIENT,
                data = new DeleteIngredientResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    warehouseList = Warehouse.GetInstance()._ingredients.Values.ToList()
                }
            });
        }


        public void PlaceOrder(DataPacket packet)
        {

            Dictionary<int, List<string>> pizzaOrder = packet.GetData<PlaceOrderRequestPacket>().pizzaOrder;
            PizzaOrder _pizzaOrder = new();

            _pizzaOrder.OrderId = (uint)new Random().Next(0, 1000);
            _pizzaOrder.Status = OrderStatus.ORDERED;
            kitchen.orderPizza(pizzaOrder);

            if (kitchen._orderComplete)
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

            Kitchen.GetInstance().AllOrders.Add(_pizzaOrder);
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<PlaceOrderResponsePacket>
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    orderList = Kitchen.GetInstance().AllOrders
                }
            });
        }

        public void GetList(DataPacket packet)
        {
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<GetListResponsePacket>
            {
                type = PacketType.GET_LIST,
                data = new GetListResponsePacket()
                {
                    allItems = Warehouse.GetInstance()._ingredients.Values.ToList()
                }
            });
        }
    }
}
