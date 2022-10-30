using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Login;
using System;
using System.Net;
using System.Net.Sockets;

namespace Pizza_Server.Logic.Connections
{
    public class ConnectionHandler
    {

        private readonly Server _server;
        private readonly TcpListener _tcpListener;
        private readonly OperationHandler _operationHandler;

        public ConnectionHandler(Server server, int port)
        {
            _server = server;
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
                Console.WriteLine(print);

                Guid authenticationID = Guid.NewGuid();

                Client client = new Client(tcpClient, _operationHandler.HandleDataCallback, authenticationID);
                client.BeginRead();

                client.SendData(new DataPacket<AutenticationPacket>()
                {
                    type = PacketType.AUTHENTICATION,
                    senderID = authenticationID,
                    data = new AutenticationPacket()
                    {
                        autenticationID = authenticationID
                    }
                });

                _server.AddClient(authenticationID, client);
            }
        }
    }
}
