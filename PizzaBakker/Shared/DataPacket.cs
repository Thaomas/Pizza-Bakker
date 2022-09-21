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

    public class LoginPacket : DAbstract
    {
        public uint username;
        public string password;
    }

    public class LoginResponsePacket : DAbstract
    {
        public StatusCode statusCode;
    }

    public class ChangeStatusPacket : DAbstract
    {
        public OrderStatus newOrderStatus;
        public Guid orderID;
    }

    public class StatusPacket : DAbstract
    {
        public OrderStatus orderStatus;
        public Guid orderID;
    }

    public class AutenticationPacket : DAbstract
    {
        public Guid autenticationID;
    }


}


