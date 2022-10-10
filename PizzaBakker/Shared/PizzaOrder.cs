using System;
using System.Collections.Generic;

namespace Shared
{
    public class PizzaOrder
    {
        public Dictionary<string, List<Ingredient>> AllPizzas { get; set; }
        public string Name { get; set; }
        public Guid OrderID { get; set; }
        
        public PizzaOrder()
        {
            AllPizzas = new();
        }
    }
}