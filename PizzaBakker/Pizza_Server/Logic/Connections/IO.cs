using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REI_Server.Logic.Connections
{
    public class IO
    {

        private static readonly string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public static void WriteFile(string fileName, string content)
        {
            string path = $"{dir}{"\\"}{fileName}";

            File.WriteAllText(path, content);
        }

        /*public static List<ICommon> ReadObjectFromFile<T>(string fileName)
        {
            string path = $"{dir}{"\\"}{fileName}";

            if (!File.Exists(path))
            {
                return default(T);
            }
        }*/

        public static string ReadFile(string fileName)
        {
            string path = $"{dir}{"\\"}{fileName}";

            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }     
    }
}
