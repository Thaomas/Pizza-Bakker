using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Main;
using Shared;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public abstract class OpHndlrAbstract
    {

        protected Dictionary<PacketType, Action<DataPacket>> OperationHandler;
        protected Server _server;

        public void Execute(DataPacket p, Client c)
        {
            if (OperationHandler.ContainsKey(p.type))
                OperationHandler[p.type](p);
        }
    }
}
