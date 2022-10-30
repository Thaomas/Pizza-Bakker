using Newtonsoft.Json;
using System;
using System.IO;

namespace Pizza_Server.Logic.Connections
{
    public class IO
    {
        private static readonly string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public static void WriteFile(string fileName, string content)
        {
            string path = $"{dir}{"\\"}{fileName}";

            File.WriteAllText(path, content);
        }

        public static T ReadObjectFromFile<T>(string fileName)
        {
            string path = $"{dir}{"\\"}{fileName}";

            if (!File.Exists(path))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
