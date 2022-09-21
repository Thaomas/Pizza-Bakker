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

        private readonly Server _server;
        private readonly Random _random;
        private readonly TcpListener _tcpListener;
        private readonly OperationHandler _operationHandler;

        public ConnectionHandler(Server server, int port)
        {
            _server = server;
            _random = new Random();
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _operationHandler = new OperationHandler(_server);

            _tcpListener.Start();
        }

        public void Run()
        {
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClientAsync().Result;
                string print = $"New client connected: {tcpClient.Client.RemoteEndPoint}";
                Trace.WriteLine(print);
                Console.WriteLine(print);

                Employee employee = new(tcpClient, _operationHandler.HandleDataCallback);
                employee.BeginRead();

                Guid authenticationID = Guid.NewGuid();
                employee.SendData(new DataPacket<AutenticationPacket>()
                {
                    type = PacketType.AUTHENTICATION,
                    data = new AutenticationPacket()
                    {
                        autenticationID = authenticationID
                    }
                });

                _server.AddClient(authenticationID, employee);
            }
        }
    }
}
