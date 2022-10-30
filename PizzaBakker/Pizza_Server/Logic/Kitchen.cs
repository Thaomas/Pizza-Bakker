using Newtonsoft.Json;
using Pizza_Server.Logic.Connections;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Server.Logic
{
    public class Kitchen
    {
        private static Kitchen _singleton;
        private static readonly string _saveLocation = @"SaveData\PizzaOrders.json";
        public DateTime NewestOrderDateTime;
        private List<PizzaOrder> AllOrders;
        private Customer _customer;
        private Warehouse _warehouse;

        private Kitchen()
        {
            LoadFromFile();
            _warehouse = Warehouse.Instance;
            _customer = Customer.Instance;
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

        public List<PizzaOrder> GetOrderHistory(Guid id)
        {
            return AllOrders.Where(o => o.CustomerID == id).ToList();
        }

        public void GetPizzaOrders(out List<PizzaOrder> orders)
        {
            orders = AllOrders;
        }

        public bool orderPizzas(List<string> orderPizzas, Guid customerID)
        {
            Dictionary<uint, uint> ingredientCount;

            bool enoughIngredients = checkIngredient(orderPizzas, out ingredientCount);
            Console.WriteLine($"Enough ingredients: {enoughIngredients}");
            if (!enoughIngredients)
                return false;

            foreach (var pair in ingredientCount)
            {
                _warehouse.Ingredients[pair.Key].Count -= pair.Value;
            }

            PizzaOrder pizzaOrder = new()
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = (uint)new Random().Next(0, 1000),
                Status = OrderStatus.ORDERED,
                CustomerID = customerID,
                AllPizzas = orderPizzas
            };
            _warehouse.listChanged();
            AddOrder(pizzaOrder);

            return true;
        }

        public bool checkIngredient(List<string> pizzaOrder, out Dictionary<uint, uint> ingredientCount)
        {
            ingredientCount = new Dictionary<uint, uint>();
            foreach (string singlePizza in pizzaOrder)
            {
                Pizza foundPizza = _customer._pizzas.Find(c => c.Name == singlePizza);
                foreach (uint ingredientID in foundPizza.Ingredients)
                {
                    if (!ingredientCount.ContainsKey(ingredientID))
                        ingredientCount.Add(ingredientID, 0);
                    ingredientCount[ingredientID]++;
                }
            }

            foreach (var pair in ingredientCount)
            {
                WarehouseItem retrievedIngredient = Warehouse.Instance.Ingredients[pair.Key];
                if (pair.Value > retrievedIngredient.Count)
                    return false;
            }
            return true;
        }

        public void AddOrder(PizzaOrder pizzaOrder)
        {
            AllOrders.Add(pizzaOrder);
            ListChanged();
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

        public void LoadFromFile()
        {
            AllOrders = IO.ReadObjectFromFile<List<PizzaOrder>>(_saveLocation);
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
            IO.WriteFile(_saveLocation, serializeData);
        }
    }
}