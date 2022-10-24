using System;
using System.Collections.Generic;
using Pizza_Server.Logic.Connections;
using Shared;

namespace Pizza_Server.Logic
{
    public class Customer
    {
        public static Customer _singleton;
        private List<Pizza> _pizzas = new();
        public List<Pizza> pizzas { get => _pizzas; }
    
        
        private Customer()
        {
            LoadFromFile();
        }
        
        public static Customer Instance
        {
            get {
                if (_singleton == null)
                    _singleton = new Customer();
                return _singleton;
            }
        }
        
        public void LoadFromFile()
        {
            _pizzas = IO.ReadObjectFromFile<List<Pizza>>("SaveData\\Pizzas.json");
        }
    }
}