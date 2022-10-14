using Newtonsoft.Json;
using Pizza_Server.Logic.Connections;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

//Hij kon de klasse niet vinden omdat de namespace dezelde naam had
namespace Pizza_Server.Logic.WarehouseNS
{
    public class Warehouse
    {
        public Dictionary<uint, WarehouseItem> _ingredients = new();
        private static Warehouse _singleton;

        private Warehouse()
        {
            LoadFromFile();
        }

        public static Warehouse GetInstance()
        {
            if (_singleton == null)
                _singleton = new Warehouse();
            return _singleton;
        }

        public void SaveIngredients()
        {
            string serializeData = JsonConvert.SerializeObject(_ingredients.Values.ToList(), Formatting.Indented);
            IO.WriteFile("SaveData\\Warehouse.json", serializeData);
        }
        
        public void LoadFromFile()
        {
            List<WarehouseItem> list = IO.ReadObjectFromFile<List<WarehouseItem>>("SaveData\\Warehouse.json");

            _ingredients = new Dictionary<uint, WarehouseItem>();

            list.ForEach(i => _ingredients.Add(i.Ingredient.Id, i));

            if (_ingredients == null) {
                Console.WriteLine("Geen ingredienten beschikbaar!");
            }
        }
    }
}
