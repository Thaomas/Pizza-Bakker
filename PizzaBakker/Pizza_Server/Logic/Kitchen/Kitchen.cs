using System;
using System.Collections.Generic;
using System.Linq;
using Pizza_Server.Logic.WarehouseNS;
using Shared;

namespace Pizza_Server.Logic.Kitchen
{
    public class Kitchen
    {
        public bool _orderComplete = true; 
        
        public Kitchen()
        {
            
        }

        /*public static Kitchen GetInstance()
        {
            if (_singleton == null)
                _singleton = new Kitchen();
            return _singleton;
        }*/
        
        public void orderPizza(List<string> orderPizza)
        {
            
            List<string> _outOfStockIngredients = new();

            for (int i = 1; i < orderPizza.Count; i++)
            {
                if (!checkIngredient(orderPizza[i]))
                {
                    _orderComplete = false;
                    
                    if (!_outOfStockIngredients.Contains(orderPizza[i]))
                    {
                        _outOfStockIngredients.Add(orderPizza[i]);
                    }
                }
            }
            //TODO eerst checkken of alle bestelingen valid zijn en dan pas substracten
            if (_orderComplete)
            {
//                foreach (string singleIngedient in orderPizza)
                for (int i = 1; i < orderPizza.Count; i++)
                {
                    Console.WriteLine(orderPizza[i]);
                    WarehouseItem retrievedIngredient = Warehouse.GetInstance()._ingredients.Values
                        .First(name => name.Ingredient.Name.Equals(orderPizza[i]));

                    retrievedIngredient.Count -= 1;

                    Warehouse.GetInstance()._ingredients[retrievedIngredient.Ingredient.Id] = retrievedIngredient;
                }
            } else  {

                foreach (string singleIngedient in _outOfStockIngredients)
                {
                    Console.WriteLine(singleIngedient);
                }
            }
        }
        
        public bool checkIngredient(string singleIngredient)
        {
            WarehouseItem retrievedIngredient = Warehouse.GetInstance()._ingredients.Values.First(name => name.Ingredient.Name.Equals(singleIngredient));
            
            if (retrievedIngredient.Count > 0) {
                return true;
            } else {
                return false;
            }                
        }
    }
}