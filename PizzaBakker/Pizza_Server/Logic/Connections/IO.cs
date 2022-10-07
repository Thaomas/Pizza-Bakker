﻿using Newtonsoft.Json;
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
        public static void WriteFile(string fileName, string fileExtension, string content, bool append = false)
        {
            string path = $"{dir}{"\\"}{fileName}{fileExtension}";

            if (append)
            {
                File.AppendAllText(path, content);
                return;
            }

            File.WriteAllText(path, content);
        }

        public static void WriteFile(string fileName, string content)
        {
            string path = $"{dir}{"\\"}{fileName}";

            File.WriteAllText(path, content);
        }

        public static void WriteNewTextToFile(string fileName, string content)
        {
            string path = $"{dir}{"\\"}{fileName}";
            if (File.Exists(path))
            {
                File.WriteAllText(path, content);
                return;
            }
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

        public static string ReadFile(string fileName)
        {
            string path = $"{dir}{"\\"}{fileName}";

            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }

        public static void SaveEmployees(Dictionary<uint, Employee> dic)
        {
            WriteFile("SaveData\\Employees", ".json", JsonConvert.SerializeObject(dic, Formatting.Indented));
        }

        public static Dictionary<uint, Employee> LoadEmployees()
        {
            string jsonString = ReadFile("SaveData\\Employees.json");

            if (jsonString == null)
            {
                return new Dictionary<uint, Employee>();
            }

            return JsonConvert.DeserializeObject<Dictionary<uint, Employee>>(jsonString);
        }
    }
}
