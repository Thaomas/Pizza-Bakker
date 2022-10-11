using System;
using System.Collections.Generic;

namespace Shared
{
    public class PizzaOrder
    {
        public List<string> AllPizzas { get; set; }
        public string Name { get; set; }
        public Guid OrderID { get; set; }
        
        public PizzaOrder()
        {
            AllPizzas = new();
        }
    }
}