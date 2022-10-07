using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
        public Guid senderID;
        public PacketType type;
        public T data;
    }

    public class DataPacket : DAbstract
    {
        public Guid senderID;
        public PacketType type;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private JObject data;

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
        public class AddIngredientPacket : DAbstract
        {
            public string message;
        }
        
        public class GetListRequestPacket : DAbstract
        {
            
        }
        
        public class GetListResponsePacket : DAbstract
        {
            public List<WarehouseItem> allItems;
        }
    }
}


