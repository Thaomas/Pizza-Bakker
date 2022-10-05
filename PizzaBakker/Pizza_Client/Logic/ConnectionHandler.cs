using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using Shared.Login;
using System;
using System.Net.Sockets;
using System.Text;

namespace REI.Util
{
    public class ConnectionHandler
    {
        private static ConnectionHandler instance;

        private TcpClient tcpClient;
        private NetworkStream stream;
        public Action<DataPacket> callback;

        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];

        public bool IsConnected { get => tcpClient.Connected; }

        public Guid ID { get; set; } = Guid.Empty;

        private ConnectionHandler()
        {
        }

        public static ConnectionHandler GetInstance()
        {
            if (instance is null)
            {
                instance = new ConnectionHandler();
            }

            return instance;
        }

        /// <summary>
        /// When this method is called, the client will try to connect to the server.
        /// When a connection is made it will call the method <see cref="OnConnectionMade(IAsyncResult)"/>.
        /// </summary>
        public void ConnectToServer()
        {
            callback = OnServerConnectionMade;
            tcpClient = new TcpClient();
            tcpClient.BeginConnect("localhost", 6000, OnConnectionMade, null);
        }

        private void OnServerConnectionMade(DataPacket packet)
        {
            if (packet.type == PacketType.AUTHENTICATION)
            {
                AutenticationPacket authPacket = packet.GetData<AutenticationPacket>();
                ID = authPacket.autenticationID;
                SendData(new DataPacket<AuthenticationResponsePacket>
                {
                    senderID = ID,
                    type = PacketType.AUTHENTICATION_RESPONSE,
                    data = new AuthenticationResponsePacket
                    {
                        autenticationID = ID,
                        clientType = ClientType.EMPLOYEE
                    }
                });

            }

        }

        private void OnConnectionMade(IAsyncResult ar)
        {
            if (!tcpClient.Connected) { return; }
            stream = tcpClient.GetStream();
            stream.BeginRead(lengthBytes, 0, lengthBytes.Length, OnLengthBytesReceived, null);
        }

        private void OnLengthBytesReceived(IAsyncResult ar)
        {
            int numOfBytes = stream.EndRead(ar);
            dataBuffer = new byte[BitConverter.ToInt32(lengthBytes)];
            stream.BeginRead(dataBuffer, 0, dataBuffer.Length, OnDataReceived, null);
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            stream.EndRead(ar);
            string data = Encoding.UTF8.GetString(dataBuffer);
            DataPacket dataPacket = JsonConvert.DeserializeObject<DataPacket>(data);
            callback(dataPacket);
            stream.BeginRead(lengthBytes, 0, lengthBytes.Length, OnLengthBytesReceived, null);
        }

        public void SendData(DAbstract packet)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());

            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
            stream.Flush();
        }

        public void SendData(Action<DataPacket> callback, DAbstract packet)
        {
            this.callback = callback;

            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());

            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
            stream.Flush();
        }
    }
}
