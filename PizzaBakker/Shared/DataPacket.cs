using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Shared
{
    public abstract class DAbstract
    {

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DataPacket<T> : DAbstract where T : DAbstract
    {
        public T data;
        public PacketType type;
        public Guid senderID;
    }

    public class DataPacket : DAbstract
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private JObject data;
        public PacketType type;
        public Guid senderID;

        public T GetData<T>() where T : DAbstract
        {
            return this.data.ToObject<T>();
        }
    }

    public class ErrorPacket : DAbstract
    {
        public StatusCode statusCode;
    }

    namespace Login
    {
        public class AutenticationPacket : DAbstract
        {
            public Guid autenticationID;
        }

        public class AuthenticationResponsePacket : DAbstract
        {
            public Guid autenticationID;
            public ClientType clientType;
        }

        public class LoginPacket : DAbstract
        {
            public uint username;
            public string password;
            public ClientType clientType;
        }
        public class LoginResponsePacket : DAbstract
        {
            public StatusCode statusCode;
        }
    }

    namespace Order
    {
        public class GetOrdersPacket : DAbstract
        {
            public Dictionary<Guid, PizzaOrder> orders;
        }

        public class ChangeStatusPacket : DAbstract
        {
            public OrderStatus orderStatus;
            public Guid orderID;
        }

        public class StatusPacket : DAbstract
        {
            public OrderStatus orderStatus;
            public Guid orderID;
        }
    }

    namespace Warehouse
    {
        public class AddIngredientRequestPacket : DAbstract
        {
            public WarehouseItem ingredient;
        }

        public class AddIngredientResponsePacket : DAbstract
        {
            public List<WarehouseItem> warehouseList;
            public StatusCode statusCode;
        }

        public class GetListRequestPacket : DAbstract { }

        public class GetListResponsePacket : DAbstract
        {
            public List<WarehouseItem> allItems;
        }

        public class DeleteIngredientRequestPacket : DAbstract
        {
            public WarehouseItem singleIngredient;
            public uint ID
            {
                get => singleIngredient.Ingredient.Id;
            }
        }

        public class DeleteIngredientResponsePacket : DAbstract
        {
            public List<WarehouseItem> warehouseList;
            public StatusCode statusCode;
        }
    }

    namespace Kitchen
    {
        public class PlaceOrderRequestPacket : DAbstract
        {
            public Dictionary<int, List<string>> pizzaOrder;
        }

        public class PlaceOrderResponsePacket : DAbstract
        {
            public PizzaOrder order;
            public StatusCode statusCode;
        }
    }
}