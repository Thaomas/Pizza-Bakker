using Newtonsoft.Json;
using Shared;
using Shared.Packet;
using Shared.Packet.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Customer_Client.Logic
{
    public class ConnectionHandler
    {
        private static ConnectionHandler _instance;

        private TcpClient tcpClient;
        private NetworkStream stream;
        public Dictionary<PacketType, Action<DataPacket>> callbacks;
        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];
        public bool IsConnected { get; set; } = false;

        public Guid ID { get; set; } = Guid.Empty;
        public Guid CustomerID { get; set; } = Guid.Empty;

        private ConnectionHandler()
        {
            callbacks = new Dictionary<PacketType, Action<DataPacket>>();
        }

        public static ConnectionHandler GetInstance()
        {
            if (_instance is null)
                _instance = new ConnectionHandler();

            return _instance;
        }

        public void ConnectToServer()
        {
            callbacks.Add(PacketType.AUTHENTICATION, OnServerConnectionMade);

            Task.Run(async () =>
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync("localhost", 6000);

                while (tcpClient.Connected)
                {
                    string data = await ReadPacket();
                    DataPacket dataPacket = JsonConvert.DeserializeObject<DataPacket>(data);

                    if (callbacks.ContainsKey(dataPacket.type))
                    {
                        callbacks[dataPacket.type](dataPacket);
                        callbacks.Remove(dataPacket.type);
                    }
                }


            });
        }
        private async Task<string> ReadPacket()
        {
            stream = tcpClient.GetStream();
            await stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);

            int length = BitConverter.ToInt32(lengthBytes, 0);

            dataBuffer = new byte[length];
            await stream.ReadAsync(dataBuffer, 0, length);

            return Encoding.UTF8.GetString(dataBuffer);
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
                    clientType = ClientType.CUSTOMER
                }
            });
            this.IsConnected = true;
        }

        

        private async void OnLengthBytesReceived(IAsyncResult ar)
        {
            try
            {
                int numOfBytes = stream.EndRead(ar);
                dataBuffer = new byte[BitConverter.ToInt32(lengthBytes)];
                var read = await stream.ReadAsync(dataBuffer, 0, dataBuffer.Length);

            }
            catch (IOException e)
            {
                //Hoort niet zo, maar geen tijd
                Environment.Exit(0);
            }
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            stream.EndRead(ar);

            string data = Encoding.UTF8.GetString(dataBuffer);
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
                packet.senderID = this.ID;

            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());
            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
            stream.Flush();
        }

        public void SendData<T>(DataPacket<T> packet, Action<DataPacket> callback) where T : DAbstract
        {
            /*this.callbacks.Add(packet.type, callback);
            this.SendData(packet);*/
            if (!this.callbacks.ContainsKey(packet.type))
            {
                this.callbacks.Add(packet.type, callback);
                this.SendData(packet);
            }
        }
    }
}
