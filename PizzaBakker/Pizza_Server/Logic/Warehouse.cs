using Newtonsoft.Json;
using Pizza_Server.Logic.Connections;
using Shared;
using Shared.Packet.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic
{
    public class Warehouse
    {
        private DateTime _newestChange;
        public DateTime NewestChange { get => _newestChange; }
        private Dictionary<uint, WarehouseItem> _ingredients = new();
        public Dictionary<uint, WarehouseItem> Ingredients { get => _ingredients; }
        private static Warehouse _singleton;

        private Warehouse()
        {
            LoadFromFile();
        }

        public static Warehouse Instance
        {
            get
            {
                if (_singleton == null)
                    _singleton = new Warehouse();
                return _singleton;
            }
        }

        public void addIngredient(AddIngredientRequestPacket addPacket)
        {
            uint id = _ingredients.Keys.Max();
            string name = addPacket.ingredient.Ingredient.Name;

            try
            {
                if (_ingredients.Values.All(v => v.Ingredient.Name != name))
                {
                    if (_ingredients.TryGetValue(id, out WarehouseItem dd))
                    {
                        uint total = id + 1;
                        addPacket.ingredient.Ingredient.Id = total;
                        _ingredients.Add(total, addPacket.ingredient);
                    }
                    else
                    {
                        addPacket.ingredient.Ingredient.Id = 1;
                        Instance._ingredients.Add(1, addPacket.ingredient);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void UpdateIngredient(uint id, string name, decimal price, uint count)
        {
            WarehouseItem updateIngredient = _ingredients.Values.First(x => x.Ingredient.Id == id);
            if (updateIngredient == null)
                return;

            updateIngredient.Count = count;
            updateIngredient.Ingredient.Name = name;
            updateIngredient.Ingredient.Price = price;
            listChanged();
        }

        public void DeleteIngredient(uint id)
        {
            _ingredients.Remove(id);
            listChanged();
        }

        public void GetList(out List<WarehouseItem> ingredients)
        {
            ingredients = _ingredients.Values.ToList();
        }

        public void listChanged()
        {
            _newestChange = DateTime.Now;
        }

        public void SaveIngredients()
        {
            string serializeData = JsonConvert.SerializeObject(_ingredients.Values.ToList(), Formatting.Indented);
            IO.WriteFile("SaveData\\Warehouse.json", serializeData);
        }

        public void LoadFromFile()
        {
            _ingredients = new Dictionary<uint, WarehouseItem>();

            IO.ReadObjectFromFile<List<WarehouseItem>>("SaveData\\Warehouse.json")
                .ForEach(i => _ingredients.Add(i.Ingredient.Id, i));
            _newestChange = DateTime.Now;

            if (_ingredients == null)
            {
                Console.WriteLine("Geen ingredienten beschikbaar!");
            }
        }
    }
}
