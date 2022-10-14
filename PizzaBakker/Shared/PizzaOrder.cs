using System;
using System.Collections.Generic;

namespace Shared
{
    public class PizzaOrder
    {
        public List<string> AllPizzas { get; set; }
        public uint OrderId { get; set; }
        //public Guid CustomerID { get; set; }
        public uint CustomerID { get; set; }
        public OrderStatus status { get; set; }
        
        public PizzaOrder()
        {
            AllPizzas = new();
        }

        public override string ToString()
        {
            return OrderId.ToString();
        }
    }
}