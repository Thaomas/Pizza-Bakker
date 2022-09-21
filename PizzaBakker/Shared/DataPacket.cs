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
        public uint senderID;
        public PacketType type;
        public T data;
    }

    public class DataPacket : DAbstract
    {
        public uint senderID;
        public PacketType type;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private JObject data;

        public DataPacket<T> GetData<T>() where T : DAbstract
        {
            return new DataPacket<T>
            {
                senderID = this.senderID,
                type = this.type,
                data = this.data.ToObject<T>()
            };
        }
    }

    public class LoginPacket : DAbstract
    {
        public string username;
        public string password;
    }
}
