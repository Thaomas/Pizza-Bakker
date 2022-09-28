using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using Shared.Login;
using System;
using System.Net.Sockets;
using System.Text;

namespace Pizza_Server.Logic.Connections.Types
{
    public class Client : IDisposable
    {
        private readonly NetworkStream stream;
        public Action<DataPacket, Client> Callback { get; set; }
        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];
        public ClientType ClientType { get; set; }
        public uint? EmployeeID { get; set; } 

        public Client(TcpClient client, Action<DataPacket, Client> callback)
        {
            stream = client.GetStream();
            this.Callback = callback;
            this.EmployeeID = null;
        }

        public void BeginRead()
        {
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
            DataPacket packet = JsonConvert.DeserializeObject<DataPacket>(data);
            Callback(packet, this);
            stream.BeginRead(lengthBytes, 0, lengthBytes.Length, OnLengthBytesReceived, null);
        }

        public void SendData(DAbstract abstractPacket)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(abstractPacket.ToJson());

            stream.Write(BitConverter.GetBytes(dataBytes.Length));
            stream.Write(dataBytes);
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public override string ToString()
        {
            return stream.Socket.RemoteEndPoint.ToString();
        }
    }
}
