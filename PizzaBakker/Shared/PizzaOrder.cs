using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Shared
{
    public class PizzaOrder
    {
        public List<string> AllPizzas { get; set; }
        public Guid OrderId { get; set; }
        public uint OrderNumber { get; set; }
        public Guid CustomerID { get; set; }
        public OrderStatus Status { get; set; }

        public PizzaOrder()
        {
            AllPizzas = new();
        }

        public string Title => $"Order {OrderNumber.ToString()}";

        public override string ToString()
        {
            return $"Order {OrderNumber.ToString()} | Pizza's: {AllPizzas.Count}";
        }

        //TODO DEBUG CODE
        public PizzaOrder Clone() => JsonConvert.DeserializeObject<PizzaOrder>(JsonConvert.SerializeObject(this));
    }
}