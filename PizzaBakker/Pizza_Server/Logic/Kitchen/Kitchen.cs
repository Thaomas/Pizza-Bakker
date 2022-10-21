using Newtonsoft.Json;
using Pizza_Server.Logic.Connections;
using Pizza_Server.Logic.WarehouseNS;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Pizza_Server.Logic
{
    public class Kitchen
    {
        private static Kitchen _singleton;
        public DateTime NewestOrderDateTime;
        public List<PizzaOrder> AllOrders;

        private Kitchen()
        {
            LoadFromFile();
        }

        public static Kitchen Instance
        {
            get
            {
                if (_singleton == null)
                    _singleton = new Kitchen();
                return _singleton;
            }
        }

        public bool _orderComplete = true;

        private Dictionary<uint, WarehouseItem> copyWarehouseItems;
        List<string> _outOfStockIngredients = new();

        public void orderPizza(Dictionary<int, List<string>> orderPizza)
        {

            Dictionary<string, int> ingredientCounter = new();

            foreach (var pizza in orderPizza.Values)
            {
                for (int i = 1; i < pizza.Count; i++)
                {

                    if (ingredientCounter.ContainsKey(pizza[i]))
                    {
                        ingredientCounter[pizza[i]] += 1;
                    }
                    else
                    {
                        ingredientCounter.Add(pizza[i], 1);
                    }
                }
            }

            if (!checkIngredient(ingredientCounter))
            {
                _orderComplete = false;
            }

            if (_orderComplete)
            {
                foreach (var saved in ingredientCounter.Keys)
                {
                    uint ingredientcount = (uint)ingredientCounter.FirstOrDefault(x => x.Key == saved).Value;

                    WarehouseItem retrievedIngredientt = Warehouse.GetInstance()._ingredients.Values.First(name => name.Ingredient.Name.Equals(saved));

                    retrievedIngredientt.Count -= ingredientcount;

                    Warehouse.GetInstance()._ingredients[retrievedIngredientt.Ingredient.Id] = retrievedIngredientt;
                }
            }
        }

        public void ChangeOrderStatus(Guid orderId, OrderStatus status)
        {
            try
            {
                AllOrders.First(p => p.OrderId == orderId).Status = status;
                ListChanged();
            }
            catch (InvalidOperationException ex) { };
        }

        public bool checkIngredient(Dictionary<string, int> singleIngredient)
        {
            bool _orderRight = true;
            WarehouseItem retrievedIngredient;

            foreach (var saved in singleIngredient.Keys)
            {
                int _pizzaInputCount = singleIngredient[saved];

                retrievedIngredient = Warehouse.GetInstance()._ingredients.Values.First(name => name.Ingredient.Name.Equals(saved));

                if (_pizzaInputCount > retrievedIngredient.Count)
                {
                    _orderRight = false;
                    retrievedIngredient.Count = 0;
                    Warehouse.GetInstance()._ingredients[retrievedIngredient.Ingredient.Id] = retrievedIngredient;

                    if (!_outOfStockIngredients.Contains(saved))
                    {
                        _outOfStockIngredients.Add(saved);
                    }
                }
            }
            return _orderRight;
        }

        public void LoadFromFile()
        {
            AllOrders = IO.ReadObjectFromFile<List<PizzaOrder>>("SaveData\\PizzaOrders.json");
            AllOrders.ForEach(e => e.OrderNumber = (e.OrderNumber == 0) ? (uint)new Random().Next(0, 1000) : e.OrderNumber);
            NewestOrderDateTime = DateTime.Now;
            if (AllOrders == null)
            {
                Console.WriteLine("Geen orders beschikbaar!");
            }
        }


        private void ListChanged()
        {
            NewestOrderDateTime = DateTime.Now;
        }

        public void SaveOrders()
        {
            string serializeData = JsonConvert.SerializeObject(AllOrders, Formatting.Indented);
            IO.WriteFile("SaveData\\PizzaOrders.json", serializeData);
        }
    }
}