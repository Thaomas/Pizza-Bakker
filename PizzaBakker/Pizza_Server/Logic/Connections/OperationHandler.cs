using Newtonsoft.Json.Linq;
using Pizza_Server.Logic.Connections.Types;
using REI_Server.Models;
using REI_Server.ViewModels;
using Shared;
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
                        callback = (DataPacket p, Client c) => {_customerOperationHandlers[p.type](packet)};
                        break;
                    case ClientType.BAKER:
                        callback = (DataPacket p, Client c) => {_bakerOperationHandlers[p.type](packet)};
                        break;
                    case ClientType.WAREHOUSE:
                        callback = (DataPacket p, Client c) => {_warehouseOperationHandlers[p.type](packet)};
                        break;
                    default:
                        callback = (DataPacket p, Client c) => { }; 
                        break;
                }
                client.Callback = callback;
            }
        }




        public void Authenticate(DataPacket packet)
        {
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
                        statusCode = StatusCode.NOT_FOUND,
                    }
                });
                return;
            }

            // Correct login info => change authentication id to the real id.
            Employee client = _server.IdToClient[authId];
            _server.IdToClient.Remove(authId);
            //?  _server.IdToClient[id] = client;

            Employee employee = _server.IdToEmployee[id];

            // Let the client know that it can log in. 
            client.SendData(new DataPacket<LoginResponsePacket>
            {
                type = PacketType.LOGIN_RESPONSE,
                data = new LoginResponsePacket()
                {
                    statusCode = StatusCode.ACCEPTED
                }
            });

            _server.Log = $"Employee: {employee.WorkId}, Logged in";
        }


        public void ChangeStatus(DataPacket packet)
        {
            ChangeStatusPacket statusPacket = packet.GetData<ChangeStatusPacket>();
            _server.IdToEmployee[jObject.Value<uint>("ID")].Status = (EmployeeStatus)jObject["Data"].Value<int>("EmployeeStatus");

            _server.Log = $"Employee: {jObject.Value<uint>("ID")}, changed their status to: {(EmployeeStatus)jObject["Data"].Value<int>("EmployeeStatus")}";
        }


        public void DeleteNote(JObject jObject)
        {
            _server.Log = $"PRIVACYLEVEL IS NU : {jObject["Data"]["PrivacyLevel"]}";
        }

    }
}
