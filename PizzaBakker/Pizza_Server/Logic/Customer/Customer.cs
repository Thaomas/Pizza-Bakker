using Pizza_Server.Logic.Connections;
using Pizza_Server.Logic.WarehouseNS;
using Shared;
using System.Collections.Generic;

namespace Pizza_Server.Logic
{
    public class Customer
    {
        public static Customer _singleton;
        private List<Pizza> _pizzas = new();


        private Customer()
        {
            LoadFromFile();
        }

        public static Customer Instance
        {
            get
            {
                if (_singleton == null)
                    _singleton = new Customer();
                return _singleton;
            }
        }

        public Dictionary<string, List<string>> getPizzas()
        {
            Dictionary<string, List<string>> pizzas = new Dictionary<string, List<string>>();

            _pizzas.ForEach(p => pizzas.Add(p.Name, getIngredients(p)));
            return pizzas;
        }

        private List<string> getIngredients(Pizza p)
        {
            List<string> ingredients = new List<string>();
            p.Ingredients.ForEach(i =>
            {
                try
                {
                    ingredients.Add(Warehouse.Instance.Ingredients[i].Ingredient.Name);
                }
                catch (KeyNotFoundException e)
                {
                    ingredients.Add($"Not found ID: {i}");
                }

            });

            return ingredients;
        }

        public void LoadFromFile()
        {
            _pizzas = IO.ReadObjectFromFile<List<Pizza>>("SaveData\\Pizzas.json");
        }
    }
}