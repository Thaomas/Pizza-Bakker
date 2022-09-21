using Newtonsoft.Json;
using REI_Server.Logic.Connections;
using REI_Server.Models;
using Shared;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace REI_Server.ViewModels
{
    public class Server
    {
        public Dictionary<uint, Client> IdToClient { get; } = new();
        public Dictionary<uint, Employee> IdToEmployee { get; } = new();

        public Dictionary<string, Note> Notes { get; } = new();

        private readonly StringBuilder _clientsStringBuilder = new StringBuilder();
        public string Clients
        {
            get => _clientsStringBuilder.ToString();
            set
            {
                _ = _clientsStringBuilder.Append(value + "\n");
            }
        }

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
            IdToEmployee = IO.LoadEmployees();
            Notes = JsonConvert.DeserializeObject<Dictionary<string, Note>>(IO.ReadFile("SaveData\\Notes.json"));

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance(this);
            new Thread(connectionHandler.Run).Start();

            new Thread(SavingLoop).Start();
        }

        public void AddClient(uint id, Client client)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IdToClient.Add(id, client);
                Clients = client.ToString();
            });
        }

        private void SavingLoop()
        {
            while (true)
            {
                IO.SaveEmployees(IdToEmployee);
                IO.WriteFile("SaveData\\Notes", ".json", JsonConvert.SerializeObject(Notes, Formatting.Indented));
                Thread.Sleep(10000);
            }
        }
    }
}
