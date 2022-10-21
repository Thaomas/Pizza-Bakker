using Pizza_Server.Logic.Connections.OperationHandlers;
using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Login;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections
{
    class OperationHandler
    {
        private readonly Server _server;
        private Kitchen kitchen = Kitchen.Instance;

        public OperationHandler(Server viewModel)
        {
            _server = viewModel;
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
                        callback = new CustomerHandler(_server).Execute;
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
            client.ClientType = loginPacket.clientType;
            client.Callback = (client.ClientType == ClientType.BAKER) ? new BakerHandler(_server).Execute : new WarehouseHandler(_server).Execute;
            // Let the client know that it can log in. 
            client.SendData(new DataPacket<LoginResponsePacket>
            {
                type = PacketType.LOGIN,
                data = new LoginResponsePacket()
                {
                    statusCode = StatusCode.ACCEPTED,
                    clientType = client.ClientType
                }
            });

            _server.Log = $"Employee: {employee.WorkId}, Logged in as a {client.ClientType}";
        }
    }
}
