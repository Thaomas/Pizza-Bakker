using Newtonsoft.Json.Linq;
using REI_Server.Models;
using REI_Server.ViewModels;
using Shared;
using System;

namespace REI_Server.Logic.Connections
{
    class OperationHandler
    {
        private readonly Server _server;

        public OperationHandler(Server viewModel)
        {
            _server = viewModel;
        }

        public void SaveNote(JObject jObject)
        {
            if (jObject["Data"]["Content"] is null) { return; }

            if (_server.Notes.ContainsKey(jObject["Data"]["Title"].ToObject<string>()))
            { return; }


            _server.Notes.Add
                (
                    jObject["Data"]["Title"].ToObject<string>(),
                    new Note()
                    {
                        Title = jObject["Data"]["Title"].ToObject<string>(),
                        Content = jObject["Data"]["Content"].ToObject<string>()
                    }
                );

            _server.Log = $"Employee: {jObject["ID"]}, Saved a file";
        }

        public void GetNote(JObject jObject)
        {
            uint id = jObject["ID"].ToObject<uint>();
            string title = jObject["Data"]["Title"].ToObject<string>();

            if (!_server.Notes.ContainsKey(title)) { return; }

            string content = _server.Notes[title].Content;

            Client client = _server.IdToClient[id];

            client.SendData(new JsonFile
            {
                StatusCode = (int)StatusCodes.OK,
                OppCode = (int)OperationCodes.GET_NOTE,
                ID = id,
                Data = new JsonData
                {
                    Content = content
                }
            });

            _server.Log = $"Employee: {id}, Requested a file";
        }

        public void Authenticate(JObject jObject)
        {
            uint id = jObject["ID"].ToObject<uint>();
            uint authId = jObject["Data"]["AutenticationID"].ToObject<uint>();

            // Wrong login info.
            if (!_server.IdToEmployee.ContainsKey(id) ||
                _server.IdToEmployee[id].Password != jObject["Data"]["Password"].ToObject<string>())
            {
                _server.IdToClient[authId].SendData(new JsonFile
                {
                    StatusCode = (int)StatusCodes.NOT_FOUND,
                    OppCode = (int)OperationCodes.AUTHENTICATE
                });
                return;
            }

            // Correct login info => change authentication id to the real id.
            Client client = _server.IdToClient[authId];
            _server.IdToClient.Remove(authId);
            _server.IdToClient[id] = client;

            Employee employee = _server.IdToEmployee[id];

            // Let the client know that it can log in. 
            client.SendData(new JsonFile
            {
                StatusCode = (int)StatusCodes.ACCEPTED,
                OppCode = (int)OperationCodes.AUTHENTICATE,
                ID = id,
                Data = new JsonData
                {
                    Employee = employee,
                }
            });

            _server.Log = $"Employee: {employee.WorkId}, Logged in";
        }

        public void ChangeStatus(JObject jObject)
        {
            _server.IdToEmployee[jObject.Value<uint>("ID")].Status = (EmployeeStatus)jObject["Data"].Value<int>("EmployeeStatus");
            _server.Log = $"Employee: {jObject.Value<uint>("ID")}, changed their status to: {(EmployeeStatus)jObject["Data"].Value<int>("EmployeeStatus")}";
        }


        public void DeleteNote(JObject jObject)
        {


            _server.Log = $"PRIVACYLEVEL IS NU : {jObject["Data"]["PrivacyLevel"]}";
        }

    }
}
