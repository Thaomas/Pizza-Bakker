using NUnit.Framework;
using REI_Server.Models;
using REI_Server.Logic;
using System.Net.Sockets;
using System.Net;
using REI.Util;
using Newtonsoft.Json.Linq;
using System;
using Shared;

namespace Tests
{
    public class TestConnectionHandler
    {
        TcpListener _tcpListener = new TcpListener(IPAddress.Any, 6000);

        [SetUp]
        public void Setup()
        {
            _tcpListener.Start();

            REI.Util.ConnectionHandler connectionHandler = REI.Util.ConnectionHandler.GetInstance();

            connectionHandler.ConnectToServer();
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(_tcpListener.AcceptTcpClient() != null);
        }
}