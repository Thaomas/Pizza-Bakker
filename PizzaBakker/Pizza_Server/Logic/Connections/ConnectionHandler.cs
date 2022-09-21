using Newtonsoft.Json.Linq;
using REI_Server.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace REI_Server.Logic.Connections
{
    public class ConnectionHandler
    {
        private static ConnectionHandler _instance;

        private readonly Server _server;
        private readonly Random _random;
        private readonly TcpListener _tcpListener;
        private readonly OperationHandler _operationHandler;
        private readonly Dictionary<int, Action<JObject>> _operationHandlers;

        private ConnectionHandler(Server server)
        {
            _server = server;
            _random = new Random();
            _tcpListener = new TcpListener(IPAddress.Any, 6000);
            _operationHandler = new OperationHandler(_server);

            _operationHandlers = new Dictionary<int, Action<JObject>>
            {
                { (int)OperationCodes.SAVE_NOTE, _operationHandler.SaveNote },
                { (int)OperationCodes.GET_NOTE, _operationHandler.GetNote},
                { (int)OperationCodes.AUTHENTICATE, _operationHandler.Authenticate },
                { (int)OperationCodes.CHANGE_STATUS, _operationHandler.ChangeStatus},
                { (int)OperationCodes.DELETE_NOTE, _operationHandler.DeleteNote}
            };

            _tcpListener.Start();
        }

        public static ConnectionHandler GetInstance(Server server)
        {
            if (_instance is null) { _instance = new ConnectionHandler(server); }

            return _instance;
        }

        public void Run()
        {
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClientAsync().Result;
                string print = $"New client connected: {tcpClient.Client.RemoteEndPoint}";
                Trace.WriteLine(print);
                Console.WriteLine(print);

                Client client = new(tcpClient, HandleDataCallback);
                client.BeginRead();

                uint authenticationID = (uint)_random.Next();
                client.SendData(new JsonFile
                {
                    StatusCode = (int)StatusCodes.OK,
                    OppCode = (int)OperationCodes.AUTHENTICATE,
                    Data = new JsonData
                    {
                        AutenticationID = authenticationID
                    }
                });

                _server.AddClient(authenticationID, client);
            }
        }

        private void HandleDataCallback(JObject jObject)
        {
            int oppCode = jObject.Value<int>("OppCode");
            _operationHandlers[oppCode](jObject);
        }
    }
}
