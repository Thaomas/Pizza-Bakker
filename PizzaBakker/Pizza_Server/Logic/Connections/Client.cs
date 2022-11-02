using Newtonsoft.Json;
using Shared;
using Shared.Packet;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_Server.Logic.Connections.Types
{
    public class Client : IDisposable
    {
        private readonly NetworkStream stream;
        public Action<DataPacket, Client> Callback { get; set; }
        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];

        public ClientType ClientType { get; set; }
        private static PacketType[] packetBan = { PacketType.GET_ORDER_LIST, PacketType.GET_INGREDIENT_LIST };
        public readonly Guid _guid;
        private TcpClient _tcpClient;

        public Client(TcpClient client, Action<DataPacket, Client> callback, Guid id)
        {
            _tcpClient = client;
            stream = client.GetStream();
            this.Callback = callback;
            this._guid = id;
        }

        public void BeginRead()
        {
            Task.Run(async () =>
            {
                while (_tcpClient.Connected)
                {
                    string data = await ReadPacket();
                    DataPacket dataPacket = JsonConvert.DeserializeObject<DataPacket>(data);

                    if (!packetBan.Contains(dataPacket.type))
                        Console.WriteLine($"In:{dataPacket.ToJson()}");

                    Callback(dataPacket, this);
                }
                Dispose();
            });
        }
        private async Task<string> ReadPacket()
        {
            await stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);

            int length = BitConverter.ToInt32(lengthBytes, 0);

            dataBuffer = new byte[length];
            await stream.ReadAsync(dataBuffer, 0, length);

            return Encoding.UTF8.GetString(dataBuffer);
        }

        public void SendData<T>(DataPacket<T> packet) where T : DAbstract
        {
            if (packet.senderID == Guid.Empty)
                packet.senderID = _guid;

            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());

            if (!packetBan.Contains(packet.type))
                Console.WriteLine($"Out: {packet.ToJson()}");

            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
