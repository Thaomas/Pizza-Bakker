using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Shared.Packet
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
            public ClientType clientType;
        }
    }

    namespace Warehouse
    {
        public class AddIngredientRequestPacket : DAbstract
        {
            public WarehouseItem ingredient;
        }

        public class GetIngredientListRequestPacket : DAbstract
        {
            public DateTime newest;
        }

        public class GetIngredientListResponsePacket : DAbstract
        {
            public StatusCode statusCode;
            public DateTime newest;
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
        
        public class UpdateIngredientRequestPacket : DAbstract
        {
            public uint ingredientID;
            public uint count;
            public decimal price;
            public string name;
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
            public StatusCode statusCode;
        }

        public class ChangeStatusOrderRequestPacket : DAbstract
        {
            public Guid pizzaOrderId;
            public OrderStatus pizzaOrderStatus;
        }

        public class CheckOrderChangesPacket : DAbstract
        {
            public DateTime newest;
        }
        public class CheckOrderChangesResponsePacket : DAbstract
        {
            public StatusCode statusCode;
            public DateTime newest;
            public List<PizzaOrder> orders;
        }
    }
}