using Newtonsoft.Json;
using Pizza_Server.Logic.Connections.Types;
using REI_Server.Logic.Connections;
using REI_Server.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace Pizza_Server.Main
{
    public class Server
    {
        public Dictionary<Guid, Client> IdToClient { get; } = new();
        public Dictionary<uint, Employee> IdToEmployee { get; } = new();

        public Dictionary<string, Note> Notes { get; } = new();

        private readonly StringBuilder _logStringBuilder = new StringBuilder();
        public string Log
        {
            get => _logStringBuilder.ToString();
            set
            {
                _ = _logStringBuilder.Append(value + "\n");
            }
        }

        public Server()
        {
            Console.WriteLine("Starting Server...");
            IdToClient = new();
            IdToEmployee = LoadEmployees();
            Notes = JsonConvert.DeserializeObject<Dictionary<string, Note>>(IO.ReadFile("SaveData\\Notes.json"));

            ConnectionHandler connectionHandler = new ConnectionHandler(this, 6000);
            new Thread(connectionHandler.Run).Start();

            new Thread(SavingLoop).Start();

            Console.WriteLine("Server Started");
        }

        public void AddClient(Guid id, Client client)
        {
            Console.WriteLine(id);
            Console.WriteLine(client);
            IdToClient.Add(id, client);
        }

        public static void SaveEmployees(Dictionary<uint, Employee> dic)
        {
            IO.WriteFile("SaveData\\Employees.json", JsonConvert.SerializeObject(dic, Formatting.Indented));
        }

        public static Dictionary<uint, Employee> LoadEmployees()
        {
            string jsonString = IO.ReadFile("SaveData\\Employees.json");

            if (jsonString == null)
            {
                return new Dictionary<uint, Employee>();
            }

            return JsonConvert.DeserializeObject<Dictionary<uint, Employee>>(jsonString);
        }

        private void SavingLoop()
        {
            while (true)
            {
                SaveEmployees(IdToEmployee);
                IO.WriteFile("SaveData\\Notes.json", JsonConvert.SerializeObject(Notes, Formatting.Indented));
                Thread.Sleep(10000);
            }
        }
    }
}
