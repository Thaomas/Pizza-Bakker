using System;
using System.Collections.Generic;

namespace Shared
{
    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public Guid ID { get; set; }
        public List<PizzaOrder> OrderHistory { get; }

        public Customer(string name, string address)
        {
            Name = name;
            Address = address;
            ID = Guid.NewGuid();
            OrderHistory = new List<PizzaOrder>();
        }

        public bool NewOrder(PizzaOrder order)
        {
            OrderHistory.Add(order);
            

            return true;
        }
    }
}
