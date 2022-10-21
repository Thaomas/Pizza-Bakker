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

        public void AddIngredient(DataPacket packet)
        {
            AddIngredientRequestPacket addPacket = packet.GetData<AddIngredientRequestPacket>();

            uint id = Warehouse.GetInstance()._ingredients.Keys.Max();
            string name = addPacket.ingredient.Ingredient.Name;

            try
            {
                if (Warehouse.GetInstance()._ingredients.Values.All(v => v.Ingredient.Name != name))
                {
                    if (Warehouse.GetInstance()._ingredients.TryGetValue(id, out WarehouseItem dd))
                    {
                        uint total = id + 1;
                        addPacket.ingredient.Ingredient.Id = total;
                        Warehouse.GetInstance()._ingredients.Add(total, addPacket.ingredient);
                    }
                    else
                    {
                        addPacket.ingredient.Ingredient.Id = 1;
                        Warehouse.GetInstance()._ingredients.Add(1, addPacket.ingredient);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Client client = this._server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<AddIngredientResponsePacket>
            {
                type = PacketType.ADD_INGREDIENT,
                data = new AddIngredientResponsePacket()
                {
                    warehouseList = Warehouse.GetInstance()._ingredients.Values.ToList(),
                    statusCode = StatusCode.OK
                }
            });
        }
        public void UpdateIngredient(DataPacket obj)
        {
            UpdateIngredientRequestPacket updatePacket = obj.GetData<UpdateIngredientRequestPacket>();

            var updateIngredient = Warehouse.GetInstance()._ingredients.Values.First(x => x.Ingredient.Name == updatePacket.name);

            Warehouse.GetInstance()._ingredients[updateIngredient.Ingredient.Id].Count = (uint)updatePacket.count;
            Warehouse.GetInstance()._ingredients[updateIngredient.Ingredient.Id].Ingredient.Price = updatePacket.price;

            Client client = _server.IdToClient[obj.senderID];
            client.SendData(new DataPacket<UpdateIngredientResponsePacket>
            {
                type = PacketType.UPDATE_INGREDIENT,
                data = new UpdateIngredientResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    warehouseList = Warehouse.GetInstance()._ingredients.Values.ToList()
                }
            });
        }

        public void GetIngredientList(DataPacket packet)
        {
            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<GetIngredientListResponsePacket>
            {
                type = PacketType.GET_INGREDIENT_LIST,
                data = new GetIngredientListResponsePacket()
                {
                    allItems = Warehouse.GetInstance()._ingredients.Values.ToList()
                }
            });
        }
        public void DeleteIngredient(DataPacket packet)
        {
            DeleteIngredientRequestPacket deletePacket = packet.GetData<DeleteIngredientRequestPacket>();

            Warehouse.GetInstance()._ingredients.Remove(deletePacket.ID);

            Client client = _server.IdToClient[packet.senderID];
            client.SendData(new DataPacket<DeleteIngredientResponsePacket>
            {

                type = PacketType.DELETE_INGREDIENT,
                data = new DeleteIngredientResponsePacket()
                {
                    statusCode = StatusCode.OK,
                    warehouseList = Warehouse.GetInstance()._ingredients.Values.ToList()
                }
            });
        }
    }
}
