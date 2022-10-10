using NUnit.Framework;
using System.Net;
using System.Net.Sockets;

namespace Test
{
    public class TestConnectionHandler
    {
        TcpListener _tcpListener = new TcpListener(IPAddress.Any, 6000);

        [SetUp]
        public void Setup()
        {
            _tcpListener.Start();

            //ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            //connectionHandler.ConnectToServer();
        }

        [Test]
        public void TestConnectToServer()
        {
            Assert.IsTrue(_tcpListener.AcceptTcpClient() != null);
        }
    }
}