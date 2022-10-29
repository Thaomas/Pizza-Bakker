using Newtonsoft.Json;
using Pizza_Server.Logic.Connections;
using Pizza_Server.Logic.WarehouseNS;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic
{
    public class Kitchen
    {
        private static Kitchen _singleton;
        public DateTime NewestOrderDateTime;
        private List<PizzaOrder> AllOrders;
        private Customer _customer;
        public bool _orderComplete = true;



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


        private Dictionary<uint, WarehouseItem> copyWarehouseItems;

        public void GetPizzaOrders(out List<PizzaOrder> orders)
        {
            orders = AllOrders;
        }

        public void orderPizza(List<string> orderPizzas)
        {
            _customer = Customer.Instance;
            Dictionary<string, int> pizzaCounter = new();

            foreach (var pizza in orderPizzas)
            {
                if (pizzaCounter.ContainsKey(pizza)) {
                    pizzaCounter[pizza] += 1;
                } else {
                    pizzaCounter.Add(pizza, 1);
                }
            }

            if (!checkIngredient(pizzaCounter)){
                _orderComplete = false;
            }
            
          
            if (_orderComplete)
            {
                foreach (var saved in pizzaCounter.Keys)
                {
        
                    Pizza foundPizza = _customer._pizzas.Find(c => c.Name == saved);

                    foreach (uint ingredient in foundPizza.Ingredients)
                    {
                        uint ingredientcount = (uint)pizzaCounter[saved];

                        WarehouseItem retrievedIngredientt = Warehouse.Instance.Ingredients.Values.First(name => name.Ingredient.Id.Equals(ingredient));

                        retrievedIngredientt.Count -= ingredientcount;

                        Warehouse.Instance.Ingredients[retrievedIngredientt.Ingredient.Id] = retrievedIngredientt;
                    }
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

        public bool checkIngredient(Dictionary<string, int> pizzaOrder)
        {
            bool _orderRight = true;
            WarehouseItem retrievedIngredient;

            foreach (var singlePizza in pizzaOrder.Keys)
            {
                Pizza foundPizza = _customer._pizzas.Find(c => c.Name == singlePizza);

                foreach (uint ingredient in foundPizza.Ingredients)
                {
                    int _pizzaInputCount = pizzaOrder[singlePizza];

                    retrievedIngredient = Warehouse.Instance.Ingredients.Values.First(name => name.Ingredient.Id.Equals(ingredient));
                 
                    if (_pizzaInputCount > retrievedIngredient.Count)
                    {
                        _orderRight = false;
                        retrievedIngredient.Count = 0;
                        Warehouse.Instance.Ingredients[retrievedIngredient.Ingredient.Id] = retrievedIngredient; 
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

        public void AddOrder(PizzaOrder pizzaOrder)
        {
            AllOrders.Add(pizzaOrder);
            ListChanged();
            
        }
    }
}