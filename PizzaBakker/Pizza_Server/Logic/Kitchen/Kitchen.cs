using System;
using System.Collections.Generic;
using System.Linq;
using Pizza_Server.Logic.WarehouseNS;
using Shared;

namespace Pizza_Server.Logic
{
    public class Kitchen
    {
        private static Kitchen _singleton;

        private Kitchen()
        {
            
        }
        
        public static Kitchen GetInstance()
        {
            if (_singleton == null)
                _singleton = new Kitchen();
            return _singleton;
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
                    } else {
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
                    
                    if (!_outOfStockIngredients.Contains(saved)) {
                       _outOfStockIngredients.Add(saved);
                    }    
                }
            }
            return _orderRight;
        }
    }
}