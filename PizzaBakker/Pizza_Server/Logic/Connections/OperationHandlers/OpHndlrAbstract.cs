using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public abstract class OpHndlrAbstract
    {

        protected Dictionary<PacketType, Action<DataPacket>> OperationHandler;
        protected Server _server;
        protected Client _client;

        public OpHndlrAbstract(Server server, Client client)
        {
            OperationHandler = new Dictionary<PacketType, Action<DataPacket>>();
            _server = server;
            _client = client;
        }

        public void Execute(DataPacket p, Client c)
        {
            if (OperationHandler.ContainsKey(p.type))
                OperationHandler[p.type](p);
        }
    }
}
