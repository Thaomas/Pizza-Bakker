using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Logic.WarehouseNS;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class WarehouseHandler : OpHndlrAbstract
    {
        public WarehouseHandler(Server server)
        {
            _server = server;
            this.OperationHandler = new Dictionary<PacketType, Action<DataPacket>>()
            {
                { PacketType.ADD_INGREDIENT, AddIngredient},
                { PacketType.GET_INGREDIENT_LIST, GetIngredientList},
                { PacketType.DELETE_INGREDIENT, DeleteIngredient},
                { PacketType.UPDATE_INGREDIENT, UpdateIngredient}
            };
        }
        public void GetIngredientList(DataPacket packet)
        {
            GetIngredientListRequestPacket req = packet.GetData<GetIngredientListRequestPacket>();
            List<WarehouseItem> AllItems = null;
            StatusCode code = StatusCode.BAD_REQUEST;

            if (req.newest < Warehouse.Instance.NewestChange)
            {
                code = StatusCode.OK;
                Warehouse.Instance.GetList(out AllItems);
            }

            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<GetIngredientListResponsePacket>
            {
                type = PacketType.GET_INGREDIENT_LIST,
                data = new GetIngredientListResponsePacket()
                {
                    statusCode = code,
                    newest = Warehouse.Instance.NewestChange,
                    allItems = AllItems
                }
            });
        }
        public void AddIngredient(DataPacket packet)
        {
            AddIngredientRequestPacket addPacket = packet.GetData<AddIngredientRequestPacket>();
            Warehouse.Instance.addIngredient(addPacket);
        }
        public void UpdateIngredient(DataPacket obj)
        {
            UpdateIngredientRequestPacket updatePacket = obj.GetData<UpdateIngredientRequestPacket>();
            Warehouse.Instance.UpdateIngredient(updatePacket.ingredientID, updatePacket.name, updatePacket.price, updatePacket.count);
        }
        public void DeleteIngredient(DataPacket packet)
        {
            DeleteIngredientRequestPacket deletePacket = packet.GetData<DeleteIngredientRequestPacket>();

            Warehouse.Instance.DeleteIngredient(deletePacket.ID);

        }
    }
}
