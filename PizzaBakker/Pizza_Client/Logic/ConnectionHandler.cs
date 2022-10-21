using Newtonsoft.Json;
using Shared;
using Shared.Packet;
using Shared.Packet.Login;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Employee_Client.Util
{
    public class ConnectionHandler
    {
        private static ConnectionHandler _instance;

        private TcpClient tcpClient;
        private NetworkStream stream;
        public Dictionary<PacketType, Action<DataPacket>> callbacks;

        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];

        public bool IsConnected { get => tcpClient.Connected; }

        public Guid ID { get; set; } = Guid.Empty;

        private ConnectionHandler()
        {
        }

        public static ConnectionHandler GetInstance()
        {
            if (_instance is null)
                _instance = new ConnectionHandler();

            return _instance;
        }

        /// <summary>
        /// When this method is called, the client will try to connect to the server.
        /// When a connection is made it will call the method <see cref="OnConnectionMade(IAsyncResult)"/>.
        /// </summary>
        public void ConnectToServer()
        {

            callbacks = new Dictionary<PacketType, Action<DataPacket>>();

            callbacks.Add(PacketType.AUTHENTICATION, OnServerConnectionMade);

            tcpClient = new TcpClient();
            tcpClient.BeginConnect("localhost", 6000, OnConnectionMade, null);
        }

        private void OnServerConnectionMade(DataPacket packet)
        {
            AutenticationPacket authPacket = packet.GetData<AutenticationPacket>();
            ID = authPacket.autenticationID;
            SendData(new DataPacket<AuthenticationResponsePacket>
            {
                senderID = ID,
                type = PacketType.AUTHENTICATION,
                data = new AuthenticationResponsePacket
                {
                    autenticationID = ID,
                    clientType = ClientType.EMPLOYEE
                }
            });
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
            Trace.WriteLine(data);
            DataPacket dataPacket = JsonConvert.DeserializeObject<DataPacket>(data);
            if (callbacks.ContainsKey(dataPacket.type))
            {
                callbacks[dataPacket.type](dataPacket);
                callbacks.Remove(dataPacket.type);
            }

            stream.BeginRead(lengthBytes, 0, lengthBytes.Length, OnLengthBytesReceived, null);
        }

        public void SendData<T>(DataPacket<T> packet) where T : DAbstract
        {
            if (packet.senderID == Guid.Empty)
                packet.senderID = ID;

            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());
            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
            stream.Flush();
        }

        public void SendData<T>(DataPacket<T> packet, Action<DataPacket> callback) where T : DAbstract
        {
            if (!this.callbacks.ContainsKey(packet.type))
            {
                this.callbacks.Add(packet.type, callback);
                this.SendData(packet);
            }
        }
    }
}
