using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Logic.WarehouseNS;
using Pizza_Server.Main;
using Shared;
using Shared.Login;
using Shared.Order;
using Shared.Warehouse;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections
{
    class OperationHandler
    {
        private readonly Server _server;
        private readonly Dictionary<PacketType, Action<DataPacket>> _customerOperationHandlers;
        private readonly Dictionary<PacketType, Action<DataPacket>> _bakerOperationHandlers;
        private readonly Dictionary<PacketType, Action<DataPacket>> _warehouseOperationHandlers;

        public OperationHandler(Server viewModel)
        {
            _server = viewModel;
            _bakerOperationHandlers = new Dictionary<PacketType, Action<DataPacket>>
            {
                { PacketType.CHANGE_STATUS, ChangeStatus}
            };

            _warehouseOperationHandlers = new Dictionary<PacketType, Action<DataPacket>>
            {
                { PacketType.ADD_INGREDIENT, AddIngredient},
                { PacketType.GET_LIST, GetList}
            };


            _customerOperationHandlers = new();
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
                    senderID = packet.senderID,
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

            // Correct login info => change authentication id to the real id.
            //Client client = _server.IdToClient[authId];
            //_server.IdToClient.Remove(authId);
            //?  _server.IdToClient[id] = client;

            Employee employee = _server.IdToEmployee[id];
            Dictionary<PacketType, Action<DataPacket>> oppHandler = (client.ClientType == ClientType.BAKER) ? _bakerOperationHandlers : _warehouseOperationHandlers;


            client.Callback = (DataPacket p, Client c) => oppHandler[p.type](packet);
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


        public void ChangeStatus(DataPacket packet)
        {
            ChangeStatusPacket statusPacket = packet.GetData<ChangeStatusPacket>();
            Client client = _server.IdToClient[packet.senderID];
        }

        public void AddIngredient(DataPacket packet)
        {
            Console.WriteLine("server-side response");
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<AddIngredientPacket>
            {
                type = PacketType.ADD_INGREDIENT,
                data = new AddIngredientPacket()
                {
                    message = "kip boulion is toegevoegdddd"
                }
            });
        }

        public void GetList(DataPacket packet)
        {
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<GetListResponsePacket>
            {
                senderID = packet.senderID,
                type = PacketType.GET_LIST,
                data = new GetListResponsePacket()
                {

                    allItems = Warehouse.GetInstance()._ingredients
                }
            });
        }
    }
}
