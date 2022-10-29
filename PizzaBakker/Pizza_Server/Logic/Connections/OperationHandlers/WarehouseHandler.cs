using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Logic.WarehouseNS;
using Pizza_Server.Main;
using Shared;
using Shared.Packet;
using Shared.Packet.Warehouse;
using System;
using System.Collections.Generic;

namespace Pizza_Server.Logic.Connections.OperationHandlers
{
    public class WarehouseHandler : OpHndlrAbstract
    {
        private Warehouse _warehouse = Warehouse.Instance;
        public WarehouseHandler(Server server, Client client) : base(server, client)
        {
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

            if (req.newest < _warehouse.NewestChange)
            {
                code = StatusCode.OK;
                _warehouse.GetList(out AllItems);
            }

            _client.SendData(new DataPacket<GetIngredientListResponsePacket>
            {
                type = PacketType.GET_INGREDIENT_LIST,
                data = new GetIngredientListResponsePacket()
                {
                    statusCode = code,
                    newest = _warehouse.NewestChange,
                    allItems = AllItems
                }
            });
        }
        public void AddIngredient(DataPacket packet)
        {
            AddIngredientRequestPacket addPacket = packet.GetData<AddIngredientRequestPacket>();
            _warehouse.addIngredient(addPacket);
        }
        public void UpdateIngredient(DataPacket obj)
        {
            UpdateIngredientRequestPacket updatePacket = obj.GetData<UpdateIngredientRequestPacket>();
            _warehouse.UpdateIngredient(updatePacket.ingredientID, updatePacket.name, updatePacket.price, updatePacket.count);
        }
        public void DeleteIngredient(DataPacket packet)
        {
            DeleteIngredientRequestPacket deletePacket = packet.GetData<DeleteIngredientRequestPacket>();
            _warehouse.DeleteIngredient(deletePacket.ID);
        }
    }
}
