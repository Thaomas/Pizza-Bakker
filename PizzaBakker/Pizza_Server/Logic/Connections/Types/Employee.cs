using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Net.Sockets;
using System.Text;

namespace Pizza_Server.Logic.Connections.Types
{
    public class Client : IDisposable
    {
        private readonly NetworkStream stream;

        private readonly Action<JObject> callback;
        private byte[] dataBuffer;
        private readonly byte[] lengthBytes = new byte[4];

        public Client(TcpClient client, Action<JObject> callback)
        {
            stream = client.GetStream();
            this.callback = callback;

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
            JObject data = JObject.Parse(Encoding.UTF8.GetString(dataBuffer));
            callback(data);
            stream.BeginRead(lengthBytes, 0, lengthBytes.Length, OnLengthBytesReceived, null);
        }

        public void SendData(DAbstract packet)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(packet.ToJson());

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
