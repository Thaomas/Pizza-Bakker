using Newtonsoft.Json.Linq;
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
        private readonly Dictionary<PacketType, Action<DataPacket>> _operationHandlers;

        public OperationHandler(Server viewModel)
        {
            _server = viewModel;
            _operationHandlers = new Dictionary<PacketType, Action<DataPacket>>
            {
                { PacketType.CHANGE_STATUS, ChangeStatus}
            };
        }
        public void HandleDataCallback(DataPacket packet)
        {
            _operationHandlers[packet.type](packet);
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
