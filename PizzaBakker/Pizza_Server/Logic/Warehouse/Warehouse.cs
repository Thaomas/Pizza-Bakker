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

        private readonly string _fileName = $"ingredients";

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

        /*public void orderPizza(List<string> orderPizza)
        {
            bool _orderComplete = true;
            List<string> _outOfStockIngredients = new();

            foreach (string singleIngedient in orderPizza)
            {
                if (!decrementIngredient(singleIngedient))
                {
                    _orderComplete = false;
                    if (!_outOfStockIngredients.Contains(singleIngedient))
                    {
                        _outOfStockIngredients.Add(singleIngedient);
                    }

                }
            }

            if (_orderComplete)
            {
                Console.WriteLine("Bestelling is klaar gemaakt!");
            }
            else
            {
                Console.WriteLine("De bestelling kon niet afgemaatk worder ern missen een aantal ingredienten zoals: ");

                foreach (string singleIngedient in _outOfStockIngredients)
                {
                    Console.WriteLine(singleIngedient);
                }
            }
        }*/

        //WERKT!!
        /*public bool decrementIngredient(string singleIngredient)
        {

            WarehouseItem retrievedIngredient = _ingredients.First(name => name.Ingredient.Name.Equals(singleIngredient));

            if (retrievedIngredient.Count > 0)
            {
                retrievedIngredient.Count -= 1;

                _ingredients.Remove(retrievedIngredient);
                _ingredients.Add(retrievedIngredient);
                return true;
            }
            else
            {
                Console.WriteLine("Het product: " + retrievedIngredient.Ingredient.Name + " is uitverkocht!");
                return false;
            }
        }*/
    }
}
