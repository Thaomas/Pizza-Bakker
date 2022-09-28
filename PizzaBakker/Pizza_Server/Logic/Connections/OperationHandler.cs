using Newtonsoft.Json.Linq;
using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using REI_Server.Models;
using Shared;
using Shared.Login;
using Shared.Order;
using System;
using System.Collections.Generic;

namespace REI_Server.Logic.Connections
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
            _customerOperationHandlers = new();
            _warehouseOperationHandlers = new();
        }
        public void HandleDataCallback(DataPacket packet, Client client)
        {
            if(packet.type == PacketType.AUTHENTICATION_RESPONSE)
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
                client.SendData(new DataPacket<ErrorPacket>
                {
                    type = PacketType.ERROR,
                    senderID = packet.senderID,
                    data = new ErrorPacket
                    {
                        statusCode = (client.ClientType == ClientType.CUSTOMER) ? StatusCode.FORBIDDEN : StatusCode.BAD_REQUEST
                    }
                }); ;

            LoginPacket loginPacket = packet.GetData<LoginPacket>();
            uint id = loginPacket.username;
            Guid authId = packet.senderID;

            // Wrong login info.
            if (!_server.IdToEmployee.ContainsKey(id) ||
                _server.IdToEmployee[id].Password != loginPacket.password)
            {
                _server.IdToClient[authId].SendData(new DataPacket<LoginResponsePacket>
                {
                    type = PacketType.LOGIN_RESPONSE,
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
            Dictionary < PacketType, Action < DataPacket >> oppHandler = (client.ClientType == ClientType.BAKER) ? _bakerOperationHandlers : _warehouseOperationHandlers;

            client.Callback = (DataPacket p, Client c) => oppHandler[p.type](packet);
            // Let the client know that it can log in. 
            client.SendData(new DataPacket<LoginResponsePacket>
            {
                type = PacketType.LOGIN_RESPONSE,
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


    }
}
