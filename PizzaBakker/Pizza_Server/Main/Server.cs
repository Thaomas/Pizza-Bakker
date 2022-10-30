using Newtonsoft.Json;
using Pizza_Server.Logic;
using Pizza_Server.Logic.Connections;
using Pizza_Server.Logic.Connections.Types;
using Pizza_Server.Logic.WarehouseNS;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pizza_Server.Main
{
    public class Server
    {
        public Dictionary<Guid, Client> IdToClient { get; } = new();
        public Dictionary<uint, Employee> IdToEmployee { get; } = new();


        private readonly StringBuilder _logStringBuilder = new StringBuilder();
        public string Log
        {
            get => _logStringBuilder.ToString();
            set
            {
                Console.WriteLine(value);
                _ = _logStringBuilder.Append(value + "\n");
            }
        }

        public Server()
        {
            Console.WriteLine("Starting Server...");
            IdToClient = new();
            IdToEmployee = LoadEmployees();
            Warehouse wh = Warehouse.Instance;
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
            Dictionary<uint, Employee> employees = IO.ReadObjectFromFile<Dictionary<uint, Employee>>("SaveData\\Employees.json");

            if (employees == null)
            {
                return new Dictionary<uint, Employee>();
            }

            return JsonConvert.DeserializeObject<Dictionary<uint, Employee>>(employees);
        }

        private void SavingLoop()
        {
            while (true)
            {
                SaveEmployees(IdToEmployee);
                Warehouse.Instance.SaveIngredients();
                Kitchen.Instance.SaveOrders();
                Thread.Sleep(10000);
            }
        }
    }
}